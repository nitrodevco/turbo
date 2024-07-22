using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Utilities;
using Turbo.Database.Repositories.Navigator;
using Turbo.Packets.Outgoing.Handshake;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Outgoing.Room.Session;
using Turbo.Rooms.Utils;

namespace Turbo.Navigator;

public class NavigatorManager(
    IRoomManager _roomManager,
    ILogger<INavigatorManager> _logger,
    IServiceScopeFactory _serviceScopeFactory) : Component, INavigatorManager
{
    private readonly IDictionary<int, INavigatorCategory> _categories = new Dictionary<int, INavigatorCategory>();

    private readonly IDictionary<int, INavigatorEventCategory> _eventCategories =
        new Dictionary<int, INavigatorEventCategory>();

    private readonly ConcurrentDictionary<int, IPendingRoomInfo> _pendingRoomIds = new();
    private readonly IList<INavigatorTab> _tabs = new List<INavigatorTab>();

    public int GetPendingRoomId(int userId)
    {
        if (_pendingRoomIds.TryGetValue(userId, out var pendingRoomInfo)) return pendingRoomInfo.RoomId;

        return -1;
    }

    public void SetPendingRoomId(int userId, int roomId, bool approved = false)
    {
        if (userId <= 0 || roomId <= 0) return;

        _pendingRoomIds.Remove(userId, out var pendingRoomInfo);
        _pendingRoomIds.TryAdd(userId, new PendingRoomInfo
        {
            RoomId = roomId,
            Approved = approved
        });
    }

    public void ClearPendingRoomId(int userId)
    {
        _pendingRoomIds.Remove(userId, out var pendingRoomInfo);
    }

    public void ClearRoomStatus(IPlayer player)
    {
        if (player == null) return;

        ClearPendingDoorbell(player);

        var pendingRoomId = GetPendingRoomId(player.Id);

        if (pendingRoomId == -1) player.Session?.Send(new CloseConnectionMessage());
    }

    public async Task GetGuestRoomMessage(IPlayer player, int roomId, bool enterRoom = false, bool roomForward = false)
    {
        if (player == null) return;

        var room = await _roomManager.GetRoom(roomId);

        if (room == null) return;

        await player.Session.Send(new GetGuestRoomResultMessage
        {
            EnterRoom = enterRoom,
            Room = room,
            IsRoomForward = roomForward,
            IsStaffPick = false,
            IsGroupMember = false,
            AllInRoomMuted = false,
            CanMute = false
        });
    }

    public async Task EnterRoom(IPlayer player, int roomId, string password = null, bool skipState = false,
        IPoint location = null)
    {
        if (player == null || roomId <= 0) return;

        var pendingRoomId = GetPendingRoomId(player.Id);

        if (pendingRoomId == roomId) return;

        SetPendingRoomId(player.Id, roomId);

        player.ClearRoomObject();

        var room = await _roomManager.GetRoom(roomId);

        if (room != null) await room.InitAsync();

        if (room == null || room.RoomModel == null)
        {
            ClearPendingRoomId(player.Id);

            await player.Session.Send(new CantConnectMessage
            {
                Reason = CantConnectReason.Closed,
                Parameter = ""
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
                    Parameter = ""
                });

                return;
            }

            if (room.RoomDetails.UsersNow >= room.RoomDetails.UsersMax)
            {
                ClearPendingRoomId(player.Id);

                await player.Session.Send(new CantConnectMessage
                {
                    Reason = CantConnectReason.Full,
                    Parameter = ""
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
                            Parameter = ""
                        });

                        return;
                    }
                }

                #endregion
            }
        }

        ClearPendingDoorbell(player);

        SetPendingRoomId(player.Id, roomId, true);

        if (location != null) _pendingRoomIds[player.Id].Location = new Point(location);

        await player.Session.Send(new OpenConnectionMessage());
        await player.Session.Send(new RoomReadyMessage
        {
            RoomId = room.Id,
            RoomType = room.RoomModel.Name
        });
    }

    public async Task ContinueEnteringRoom(IPlayer player)
    {
        if (player == null) return;

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

        if (room == null)
            await player.Session.Send(new CantConnectMessage
            {
                Reason = CantConnectReason.Closed
            });

        if (room != null)
        {
            await room.InitAsync();

            room.EnterRoom(player, _pendingRoomIds[player.Id].Location);
        }

        ClearPendingRoomId(player.Id);
    }

    public async Task SendNavigatorCategories(IPlayer player)
    {
        await player.Session.Send(new UserFlatCatsMessage
        {
            Categories = [.. _categories.Values]
        });
    }

    public async Task SendNavigatorSettings(IPlayer player)
    {
        await player.Session.Send(new NewNavigatorPreferencesMessage
        {
            WindowX = 0,
            WindowY = 0,
            WindowWidth = 0,
            WindowHeight = 0,
            LeftPaneHidden = false,
            ResultMode = 0
        });
    }

    public async Task SendNavigatorMetaData(IPlayer player)
    {
        List<ITopLevelContext> tabs = [];

        foreach (var tab in _tabs) tabs.Add(tab.TopLevelContext);

        await player.Session.Send(new NavigatorMetaDataMessage
        {
            TopLevelContexts = tabs
        });
    }

    public async Task SendNavigatorLiftedRooms(IPlayer player)
    {
        await player.Session.Send(new NavigatorLiftedRoomsMessage());
    }

    public async Task SendNavigatorSavedSearches(IPlayer player)
    {
        await player.Session.Send(new NavigatorSavedSearchesMessage
        {
            // Todo: Implement saved searches
            SavedSearches = []
        });
    }

    public async Task SendNavigatorEventCategories(IPlayer player)
    {
        await player.Session.Send(new NavigatorEventCategoriesMessage
        {
            EventCategories = [.. _eventCategories.Values]
        });
    }

    protected override async Task OnInit()
    {
        await LoadNavigatorData();
    }

    protected override async Task OnDispose()
    {
    }

    public void ClearPendingDoorbell(IPlayer player)
    {
        if (player == null) return;

        // remove user from pending doorbells
    }

    private async Task LoadNavigatorData()
    {
        _tabs.Clear();
        _categories.Clear();
        _eventCategories.Clear();

        using var scope = _serviceScopeFactory.CreateScope();

        var navigatorRepository = scope.ServiceProvider.GetService<INavigatorRepository>();

        var tabEntities = await navigatorRepository.FindAllNavigatorTabsAsync();
        var categoryEntities = await navigatorRepository.FindAllNavigatorCategoriesAsync();
        var eventCategoriesEntities = await navigatorRepository.FindAllNavigatorEventCategoriesAsync();

        tabEntities.ForEach(entity =>
        {
            var tab = new NavigatorTab(entity);

            _tabs.Add(tab);
        });

        categoryEntities.ForEach(entity =>
        {
            var category = new NavigatorCategory(entity);

            _categories.Add(category.Id, category);
        });

        eventCategoriesEntities.ForEach(entity =>
        {
            var eventCategory = new NavigatorEventCategory(entity);

            _eventCategories.Add(eventCategory.Id, eventCategory);
        });

        _logger.LogInformation("Loaded {0} navigator tabs", _tabs.Count);
        _logger.LogInformation("Loaded {0} navigator categories", _categories.Count);
        _logger.LogInformation("Loaded {0} navigator event categories", _eventCategories.Count);
    }
}