using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Rooms;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Players;
using Turbo.Database.Repositories.Player;
using Turbo.Database.Repositories.Room;
using Turbo.Packets.Outgoing.Handshake;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Packets.Outgoing.Room.Session;
using Turbo.Players.Factories;
using Turbo.Players.Rooms;
using Turbo.Rooms.Utils;

namespace Turbo.Players;

public class PlayerManager(
    ILogger<IPlayerManager> _logger,
    IPlayerFactory _playerFactory,
    IRoomManager _roomManager,
    IServiceScopeFactory _serviceScopeFactory) : Component, IPlayerManager
{
    private readonly ConcurrentDictionary<int, IPlayer> _players = new();

    private readonly ConcurrentDictionary<int, IPendingRoomInfo> _pendingRoomIds = new();

    public ConcurrentBag<PlayerChatStyleEntity> PlayerChatStyles { get; } = new();

    public IPlayer GetPlayerById(int id)
    {
        if (id <= 0) return null;
        if (!_players.TryGetValue(id, out var value) || value is null) return null;
        return value;
    }

    public IPlayer GetPlayerByUsername(string username)
    {
        if (string.IsNullOrEmpty(username)) return null;

        foreach (var player in _players.Values)
        {
            if (player is null) continue;
            if (string.Equals(player.Name, username, StringComparison.OrdinalIgnoreCase))
                return player;
        }

        return null;
    }

    public async Task<IPlayer> GetOfflinePlayerById(int id)
    {
        var player = GetPlayerById(id);

        if (player is not null) return player;

        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();
            var playerEntity = await playerRepository.FindAsync(id);
            return playerEntity is null ? null : _playerFactory.Create(playerEntity);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error fetching offline player by ID {PlayerId}: Service not found.", id);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching offline player by ID {PlayerId}", id);
            return null;
        }
    }

    public async Task<IPlayer> GetOfflinePlayerByUsername(string username)
    {
        if (string.IsNullOrEmpty(username)) return null;

        var player = GetPlayerByUsername(username);
        if (player is not null) return player;

        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();
            var playerDTO = await playerRepository.FindUserIdAsync(username);
            if (playerDTO is null) return null;
            var playerEntity = await playerRepository.FindAsync(playerDTO.Id);
            return playerEntity is null ? null : _playerFactory.Create(playerEntity);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error fetching offline player by username '{Username}': Service not found.", username);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching offline player by username '{Username}'", username);
            return null;
        }
    }

    public async Task<IPlayer> CreatePlayer(int id, ISession session)
    {
        if (id <= 0 || session is null) return null;

        var player = await GetOfflinePlayerById(id);

        if (player is null) return null;

        if (!player.SetSession(session))
        {
            await player.DisposeAsync();
            await session.DisposeAsync();

            return null;
        }

        if (!_players.TryAdd(id, player)) return null;

        await player.InitAsync();

        return player;
    }

    public async Task RemovePlayer(int id)
    {
        if (id <= 0) return;

        var player = GetPlayerById(id);

        if (player is null) return;

        _players.Remove(id, out var removedPlayer);

        await player.DisposeAsync();
    }

    public async Task RemoveAllPlayers()
    {
        var ids = _players.Keys;
        var tasks = new List<Task>();
        foreach (var id in ids)
        {
            tasks.Add(RemovePlayer(id));
        }
        await Task.WhenAll(tasks);
    }

    public void ClearPlayerRoomStatus(IPlayer player)
    {
        if (player is null) return;

        ClearRoomStatus(player);
    }

    public async Task<string> GetPlayerName(int playerId)
    {
        var player = GetPlayerById(playerId);

        if (player is not null) return player.Name;

        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var playerRepository = scope.ServiceProvider.GetRequiredService<IPlayerRepository>();
            return (await playerRepository.FindUsernameAsync(playerId))?.Name ?? string.Empty;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error fetching player name for ID {PlayerId}: Service not found.", playerId);
            return string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching player name for ID {PlayerId}", playerId);
            return string.Empty;
        }
    }

    public async Task<IList<IPlayerBadge>> GetPlayerActiveBadges(int playerId)
    {
        if (playerId <= 0) return null;

        var player = GetPlayerById(playerId);

        if (player is null)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var playerBadgeRepository = scope.ServiceProvider.GetRequiredService<IPlayerBadgeRepository>();
                var entities = await playerBadgeRepository.FindActiveByPlayerIdAsync(playerId);
                return (IList<IPlayerBadge>)entities;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error fetching active badges for player ID {PlayerId}: Service not found.", playerId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching active badges for player ID {PlayerId}", playerId);
                return null;
            }
        }

        return player.PlayerInventory?.BadgeInventory?.ActiveBadges;
    }

    protected override async Task OnInit() => await LoadChatStyles();

    protected override async Task OnDispose() => await RemoveAllPlayers();
    public async Task LoadChatStyles()
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var chatStyleRepository = scope.ServiceProvider.GetService<IPlayerChatStyleRepository>();
        var chatStyleEntities = await chatStyleRepository.FindAllAsync();

        foreach (var entity in chatStyleEntities)
        {
            PlayerChatStyles.Add(entity);
        }

        _logger.LogInformation("Loaded {0} chat styles", PlayerChatStyles.Count);
    }

    public int GetPendingRoomId(int playerId) => _pendingRoomIds.TryGetValue(playerId, out var info) ? info.RoomId : -1;

    public void SetPendingRoomId(int playerId, int roomId, bool approved = false)
    {
        if (playerId <= 0 || roomId <= 0) return;

        _pendingRoomIds.AddOrUpdate(
            playerId,
            new PendingRoomInfo { RoomId = roomId, Approved = approved },
            (key, existingVal) => new PendingRoomInfo { RoomId = roomId, Approved = approved }
        );
    }

    public void ClearPendingRoomId(int userId) => _pendingRoomIds.Remove(userId, out var pendingRoomInfo);

    public void ClearRoomStatus(IPlayer player)
    {
        if (player is null) return;

        ClearPendingDoorbell(player);

        var pendingRoomId = GetPendingRoomId(player.Id);

        if (pendingRoomId == -1) player.Session?.Send(new CloseConnectionMessage());
    }

    public void ClearPendingDoorbell(IPlayer player)
    {
        if (player is null) return;

        // remove user from pending doorbells
    }

    public async Task OpenRoom(IPlayer player, int roomId, string password = null, bool skipState = false, IPoint location = null)
    {
        if (roomId <= 0) return;

        var pendingRoomId = GetPendingRoomId(player.Id);

        if (pendingRoomId == roomId) return;

        SetPendingRoomId(player.Id, roomId);

        player.ClearRoomObject();

        var room = await _roomManager.GetRoom(roomId);

        if (room is not null) await room.InitAsync();

        if (room is null || room.RoomModel is null)
        {
            ClearPendingRoomId(player.Id);

            await player.Session.Send(new CantConnectMessage
            {
                Reason = CantConnectReason.Closed,
                Parameter = string.Empty
            });

            return;
        }

        if (!room.RoomSecurityManager.IsOwner(player))
        {
            if (room.RoomSecurityManager.IsPlayerBanned(player))
            {
                ClearPendingRoomId(player.Id);

                await player.Session.Send(new CantConnectMessage
                {
                    Reason = CantConnectReason.Banned,
                    Parameter = string.Empty
                });

                return;
            }

            if (room.RoomDetails.UsersNow >= room.RoomDetails.UsersMax)
            {
                ClearPendingRoomId(player.Id);

                await player.Session.Send(new CantConnectMessage
                {
                    Reason = CantConnectReason.Full,
                    Parameter = string.Empty
                });

                return;
            }

            if (!skipState)
            {
                #region RoomStateType.Locked

                if (room.RoomDetails.State == RoomStateType.Locked)
                {
                    ClearPendingRoomId(player.Id);

                    // doorbell
                    // if rights do u need 2 wait
                }

                #endregion
                #region RoomStateType.Password

                else if (room.RoomDetails.State == RoomStateType.Password)
                {
                    if (!password.Equals(room.RoomDetails.Password))
                    {
                        ClearPendingRoomId(player.Id);

                        await player.Session.Send(new GenericErrorMessage
                        {
                            ErrorCode = RoomGenericErrorType.InvalidPassword
                        });

                        return;
                    }
                }

                #endregion
                #region RoomStateType.Invisible

                else if (room.RoomDetails.State == RoomStateType.Invisible)
                {
                    if (room.RoomSecurityManager.GetControllerLevel(player) == RoomControllerLevel.None)
                    {
                        ClearPendingRoomId(player.Id);

                        await player.Session.Send(new CantConnectMessage
                        {
                            Reason = CantConnectReason.Closed,
                            Parameter = string.Empty
                        });

                        return;
                    }
                }

                #endregion
            }
        }

        ClearPendingDoorbell(player);

        SetPendingRoomId(player.Id, roomId, true);

        using var scope = _serviceScopeFactory.CreateScope();
        var roomEnterLogRepository = scope.ServiceProvider.GetRequiredService<IRoomEntryLogRepository>();
        await roomEnterLogRepository.AddRoomEntryLogAsync(roomId, player.Id);

        if (location is not null)
            _pendingRoomIds[player.Id].Location = new Point(location);

        await player.Session.Send(new OpenConnectionMessage
        {
            RoomId = room.Id
        });

        await player.Session.Send(new RoomReadyMessage
        {
            RoomId = room.Id,
            RoomType = room.RoomModel.Name
        });

        if (room.RoomDetails.PaintWall != 0.0)
            await player.Session.Send(new RoomPropertyMessage
            {
                Property = RoomPropertyType.WALLPAPER,
                Value = room.RoomDetails.PaintWall.ToString()
            });

        if (room.RoomDetails.PaintFloor != 0.0)
            await player.Session.Send(new RoomPropertyMessage
            {
                Property = RoomPropertyType.FLOOR,
                Value = room.RoomDetails.PaintFloor.ToString()
            });

        await player.Session.Send(new RoomPropertyMessage
        {
            Property = RoomPropertyType.LANDSCAPE,
            Value = room.RoomDetails.PaintLandscape.ToString()
        });

        room.RoomSecurityManager.RefreshControllerLevel(player);

        await player.Session.Send(new RoomRatingMessage
        {
            CurrentScore = 0,
            CanRate = false
        });
    }

    public async Task EnterRoom(IPlayer player)
    {
        if (player is null) return;

        if (!_pendingRoomIds.ContainsKey(player.Id) || !_pendingRoomIds[player.Id].Approved)
        {
            await player.Session.Send(new CantConnectMessage
            {
                Reason = CantConnectReason.Closed
            });

            return;
        }

        var roomId = _pendingRoomIds[player.Id].RoomId;

        var room = await _roomManager.GetRoom(roomId);

        if (room is null)
            await player.Session.Send(new CantConnectMessage
            {
                Reason = CantConnectReason.Closed
            });

        if (room is not null)
        {
            await room.InitAsync();

            room.EnterRoom(player, _pendingRoomIds[player.Id].Location);
        }

        ClearPendingRoomId(player.Id);
    }
}