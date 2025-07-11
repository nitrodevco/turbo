using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Utilities;
using Turbo.Database.Repositories.Player;
using Turbo.Database.Repositories.Room;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Packets.Outgoing.Room.Permissions;
using Turbo.Packets.Outgoing.RoomSettings;
using Turbo.Packets.Outgoing.UserDefinedRoomEvents.WiredMenu;
using Turbo.Rooms.Object.Logic.Avatar;

namespace Turbo.Rooms.Managers;

public class RoomSecurityManager(
    IRoom _room,
    IPlayerManager _playerManager,
    IServiceScopeFactory _serviceScopeFactory) : Component, IRoomSecurityManager
{
    public IDictionary<int, DateTime> Bans { get; } = new ConcurrentDictionary<int, DateTime>();
    public IList<int> Rights { get; } = new List<int>();

    public bool IsStrictOwner(IRoomManipulator manipulator)
    {
        return IsStrictOwner(manipulator?.Id);
    }

    public bool IsStrictOwner(int? playerId)
    {
        if (playerId > 0 && _room.RoomDetails.PlayerId == playerId) return true;

        return false;
    }

    public bool IsOwner(IRoomManipulator manipulator)
    {
        if (manipulator == null) return false;

        if (IsStrictOwner(manipulator)) return true;

        return manipulator?.HasPermission("any_room_owner") ?? false;
    }

    public bool IsOwner(int? playerId)
    {
        if (playerId <= 0) return false;

        if (IsStrictOwner(playerId)) return true;

        var player = _playerManager.GetPlayerById((int)playerId);

        if (player != null) return player.HasPermission("any_room_owner");

        return false;
    }

    public bool IsPlayerBanned(IPlayer player)
    {
        if (player == null || !Bans.ContainsKey(player.Id)) return false;

        var isOwner = IsOwner(player);

        if (Bans.TryGetValue(player.Id, out var expiration))
        {
            if (!isOwner && DateTime.Compare(DateTime.Now, expiration) < 0) return true;

            Bans.Remove(player.Id);
        }

        return false;
    }

    public RoomControllerLevel GetControllerLevel(IRoomManipulator manipulator)
    {
        if (manipulator != null)
        {
            if (IsOwner(manipulator)) return RoomControllerLevel.Owner;

            if (_room.IsGroupRoom)
            {
                if (manipulator.HasPermission("any_group_admin")) return RoomControllerLevel.GroupAdmin;

                if (manipulator.HasPermission("any_group_member")) return RoomControllerLevel.GroupRights;

                // TODO check if the manipulator belongs to the group
            }
            else
            {
                if (manipulator.HasPermission("any_room_rights")) return RoomControllerLevel.Rights;

                if (Rights.Contains(manipulator.Id)) return RoomControllerLevel.Rights;
            }
        }

        return RoomControllerLevel.None;
    }

    public void RefreshControllerLevel(IPlayer player)
    {
        if (player == null) return;

        var controllerLevel = RoomControllerLevel.None;

        var isOwner = IsOwner(player);

        controllerLevel = GetControllerLevel(player);

        player.Session.Send(new YouAreControllerMessage
        {
            RoomId = _room.Id,
            RoomControllerLevel = controllerLevel
        });

        player.Session.Send(new WiredPermissionsMessage
        {
            CanModify = true,
            CanRead = true
        });

        if (isOwner) 
            player.Session.Send(new YouAreOwnerMessage
            {
                RoomId = _room.Id
            });
    }

    public void KickPlayer(IRoomManipulator manipulator, int playerId)
    {
        if (playerId <= 0) return;

        var player = _playerManager.GetPlayerById(playerId);

        if (player == null || !CanKickPlayer(manipulator)) return;

        if (player.RoomObject == null || player.RoomObject.Room != _room) return;

        if (player.RoomObject.Logic is PlayerLogic playerLogic) playerLogic.Kick();
    }

    public async Task BanPlayerIdWithDuration(IRoomManipulator manipulator, int playerId, double durationMs)
    {
        if (playerId <= 0 || durationMs == 0.0 || Bans.ContainsKey(playerId) ||
            !CanAdjustPlayerBan(manipulator, true) || IsOwner(playerId)) return;

        using var scope = _serviceScopeFactory.CreateScope();

        var playerRepository = scope.ServiceProvider.GetService<IPlayerRepository>();
        var roomBanRepository = scope.ServiceProvider.GetService<IRoomBanRepository>();

        var player = _playerManager.GetPlayerById(playerId);

        if (player == null)
        {
            var playerEntity = await playerRepository.FindAsync(playerId);

            if (playerEntity == null) return;
        }

        var expiration = DateTime.Now.AddMilliseconds(durationMs);

        if (!await roomBanRepository.BanPlayerIdAsync(_room.Id, playerId, expiration)) return;

        Bans.Add(playerId, expiration);

        if (player == null || player.RoomObject == null || player.RoomObject.Room != _room) return;

        if (player.RoomObject.Logic is PlayerLogic playerLogic)
            // TODO we probably need to send a banned packet/alert
            playerLogic.Kick();
    }

    public async Task AdjustRightsForPlayerId(IRoomManipulator manipulator, int playerId, bool flag)
    {
        if (manipulator != null && ((manipulator.Id != playerId && !IsOwner(manipulator)) || IsOwner(playerId))) return;

        if (Rights.Contains(playerId) == flag) return;

        using var scope = _serviceScopeFactory.CreateScope();

        var playerRepository = scope.ServiceProvider.GetService<IPlayerRepository>();
        var roomRightRepository = scope.ServiceProvider.GetService<IRoomRightRepository>();

        var playerEntity = await playerRepository.FindAsync(playerId);

        if (playerEntity == null) return;

        if (flag)
        {
            if (!await roomRightRepository.GiveRightsToPlayerIdAsync(_room.Id, playerId)) return;

            Rights.Add(playerId);

            SendOwnersComposer(new FlatControllerAddedMessage
            {
                RoomId = _room.Id,
                PlayerId = playerId,
                PlayerName = playerEntity.Name
            });
        }
        else
        {
            if (!await roomRightRepository.RemoveRightsForPlayerIdAsync(_room.Id, playerId)) return;

            Rights.Remove(playerId);

            SendOwnersComposer(new FlatControllerRemovedMessage
            {
                RoomId = _room.Id,
                PlayerId = playerId
            });
        }

        var player = _playerManager.GetPlayerById(playerId);

        if (player != null) RefreshControllerLevel(player);

        if(player.RoomObject != null) player.RoomObject.Logic.AddStatus(RoomObjectAvatarStatus.FlatControl, ((int) GetControllerLevel(player)).ToString());

    }

    public async Task RemoveAllRights(IRoomManipulator manipulator)
    {
        var playerIds = Rights.ToArray();

        if (playerIds.Length == 0) return;

        foreach (var playerId in playerIds) await AdjustRightsForPlayerId(manipulator, playerId, false);
    }

    public void SendOwnersComposer(IComposer composer)
    {
        foreach (var avatarObject in _room.RoomUserManager.AvatarObjects.RoomObjects.Values)
            if (avatarObject.RoomObjectHolder is IPlayer player)
            {
                if (!IsOwner(player)) continue;

                player.Session.Send(composer);
            }
    }

    public void SendRightsComposer(IComposer composer)
    {
        foreach (var avatarObject in _room.RoomUserManager.AvatarObjects.RoomObjects.Values)
            if (avatarObject.RoomObjectHolder is IPlayer player)
            {
                if (GetControllerLevel(player) < RoomControllerLevel.Rights) continue;

                player.Session.Send(composer);
            }
    }

    public bool CanKickPlayer(IRoomManipulator manipulator)
    {
        if (manipulator == null) return true;

        var kickType = _room.RoomDetails.KickType;

        switch (kickType)
        {
            case RoomKickType.All:
                return true;
            case RoomKickType.Rights:
                if (GetControllerLevel(manipulator) < RoomControllerLevel.Rights) return false;
                break;
            default:
                if (!IsOwner(manipulator)) return false;
                break;
        }

        return true;
    }

    public bool CanAdjustPlayerBan(IRoomManipulator manipulator, bool flag)
    {
        if (manipulator == null) return true;

        var banType = _room.RoomDetails.BanType;

        switch (banType)
        {
            case RoomBanType.Rights:
                if (GetControllerLevel(manipulator) < RoomControllerLevel.Rights) return false;
                break;
            default:
                if (!IsOwner(manipulator)) return false;
                break;
        }

        if (!flag && !IsOwner(manipulator)) return false;

        return true;
    }

    public bool CanAdjustPlayerMute(IRoomManipulator manipulator, bool flag)
    {
        if (manipulator == null) return true;

        var muteType = _room.RoomDetails.MuteType;

        switch (muteType)
        {
            case RoomMuteType.Rights:
                if (GetControllerLevel(manipulator) < RoomControllerLevel.Rights) return false;
                break;
            default:
                if (!IsOwner(manipulator)) return false;
                break;
        }

        return true;
    }

    public bool CanManipulateFurniture(IRoomManipulator manipulator, IRoomFurniture furniture)
    {
        if (furniture == null) return false;

        if (manipulator == null) return true;

        var controllerLevel = GetControllerLevel(manipulator);

        if (controllerLevel >= RoomControllerLevel.GroupAdmin) return true;

        if (_room.IsGroupRoom)
        {
            var canGroupDecorate = false;

            if (controllerLevel >= RoomControllerLevel.GroupRights && canGroupDecorate) return true;
        }
        else
        {
            if (controllerLevel >= RoomControllerLevel.Rights) return true;
        }

        return false;
    }

    public bool CanPlaceFurniture(IRoomManipulator manipulator)
    {
        if (manipulator == null) return true;

        var controllerLevel = GetControllerLevel(manipulator);

        if (controllerLevel >= RoomControllerLevel.GroupAdmin) return true;

        if (_room.IsGroupRoom)
        {
            // TODO We need to check if the group has the rights to decorate
            var canGroupDecorate = false;

            if (controllerLevel >= RoomControllerLevel.GroupRights && canGroupDecorate) return true;
        }
        else
        {
            if (controllerLevel >= RoomControllerLevel.Rights) return true;
        }

        return false;
    }

    public FurniturePickupType GetFurniturePickupType(IRoomManipulator manipulator, IRoomFurniture furniture)
    {
        if (furniture == null) return FurniturePickupType.None;

        if (manipulator == null) return FurniturePickupType.SendToOwner;

        if (manipulator.HasPermission("can_steal_furniture")) return FurniturePickupType.SendToManipulator;

        if (GetControllerLevel(manipulator) >= RoomControllerLevel.GroupAdmin) return FurniturePickupType.SendToOwner;

        return FurniturePickupType.None;
    }

    protected override async Task OnInit()
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var roomBanRepository = scope.ServiceProvider.GetService<IRoomBanRepository>();
        var roomRightRepository = scope.ServiceProvider.GetService<IRoomRightRepository>();

        var banEntities = await roomBanRepository.FindAllByRoomIdAsync(_room.Id);
        var rightEntities = await roomRightRepository.FindAllByRoomIdAsync(_room.Id);

        if (banEntities != null)
            foreach (var entity in banEntities)
            {
                if (DateTime.Compare(DateTime.Now, entity.DateExpires) >= 0)
                {
                    await roomBanRepository.RemoveBanEntityAsync(entity);

                    continue;
                }

                Bans.Add(entity.PlayerEntityId, entity.DateExpires);
            }

        foreach (var entity in rightEntities) Rights.Add(entity.PlayerEntityId);
    }

    protected override async Task OnDispose()
    {
        Bans.Clear();
        Rights.Clear();
    }
}