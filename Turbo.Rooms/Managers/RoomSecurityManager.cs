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
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Room;
using Turbo.Database.Repositories.Player;
using Turbo.Database.Repositories.Room;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Packets.Outgoing.Room.Permissions;
using Turbo.Packets.Outgoing.RoomSettings;
using Turbo.Rooms.Object.Logic.Avatar;

namespace Turbo.Rooms.Managers
{
    public class RoomSecurityManager(
        IRoom _room,
        IPlayerManager _playerManager,
        IRoomBanRepository _roomBanRepository,
        IRoomMuteRepository _roomMuteRepository,
        IRoomRightRepository _roomRightRepository) : Component, IRoomSecurityManager
    {
        public IDictionary<int, DateTime> Bans { get; private set; } = new ConcurrentDictionary<int, DateTime>();
        public IDictionary<int, DateTime> Mutes { get; private set; } = new ConcurrentDictionary<int, DateTime>();
        public IList<int> Rights { get; private set; } = new List<int>();

        protected override async Task OnInit()
        {
            var banEntities = await _roomBanRepository.FindAllByRoomIdAsync(_room.Id);
            var muteEntities = await _roomMuteRepository.FindAllByRoomIdAsync(_room.Id);
            var rightEntities = await _roomRightRepository.FindAllByRoomIdAsync(_room.Id);

            foreach (var entity in banEntities)
            {
                if (DateTime.Compare(DateTime.Now, entity.DateExpires) >= 0)
                {
                    await _roomBanRepository.RemoveBanEntityAsync(entity);

                    continue;
                }

                Bans.Add(entity.PlayerEntityId, entity.DateExpires);
            }

            foreach (var entity in muteEntities)
            {
                if (DateTime.Compare(DateTime.Now, entity.DateExpires) >= 0)
                {
                    await _roomMuteRepository.RemoveMuteEntityAsync(entity);

                    continue;
                }

                Mutes.Add(entity.PlayerEntityId, entity.DateExpires);
            }

            foreach (var entity in rightEntities) Rights.Add(entity.PlayerEntityId);
        }

        protected override async Task OnDispose()
        {
            Bans.Clear();
            Mutes.Clear();
            Rights.Clear();
        }

        public bool IsStrictOwner(IRoomManipulator manipulator)
        {
            if (manipulator != null && _room.RoomDetails.PlayerId == manipulator.Id) return true;

            return false;
        }

        public bool IsOwner(IRoomManipulator manipulator)
        {
            if (manipulator == null) return false;

            if (IsStrictOwner(manipulator)) return true;

            if (manipulator.HasPermission("any_room_owner")) return true;

            return false;
        }

        public bool IsPlayerBanned(IPlayer player)
        {
            if (player == null || !Bans.ContainsKey(player.Id)) return false;

            if (Bans.TryGetValue(player.Id, out var expiration))
            {
                if (DateTime.Compare(DateTime.Now, expiration) < 0) return true;

                Bans.Remove(player.Id);
            }

            return false;
        }

        public bool IsPlayerMuted(IPlayer player)
        {
            if (player == null || !Mutes.ContainsKey(player.Id)) return false;

            if (Mutes.TryGetValue(player.Id, out var expiration))
            {
                if (DateTime.Compare(DateTime.Now, expiration) < 0) return true;

                Mutes.Remove(player.Id);
            }

            return false;
        }

        public RoomControllerLevel GetControllerLevel(IRoomManipulator manipulator)
        {
            if (manipulator != null)
            {
                if (IsOwner(manipulator)) return RoomControllerLevel.Moderator;

                if (_room.IsGroupRoom)
                {
                    if (manipulator.HasPermission("any_group_admin")) return RoomControllerLevel.GroupAdmin;

                    if (manipulator.HasPermission("any_group_member")) return RoomControllerLevel.GroupRights;

                    // check if the manipulator belongs to the group
                }
                else
                {
                    if (manipulator.HasPermission("any_room_rights")) return RoomControllerLevel.Rights;

                    if (Rights.Contains(manipulator.Id)) return RoomControllerLevel.Rights;
                }
            }

            return RoomControllerLevel.None;
        }

        public void RefreshControllerLevel(IRoomObjectAvatar avatarObject)
        {
            if (avatarObject == null) return;

            bool isOwner = false;
            RoomControllerLevel controllerLevel = RoomControllerLevel.None;

            if (avatarObject.RoomObjectHolder is IPlayer player)
            {
                isOwner = IsOwner(player);
                controllerLevel = GetControllerLevel(player);

                player.Session.Send(new YouAreControllerMessage
                {
                    RoomId = _room.Id,
                    RoomControllerLevel = controllerLevel
                });

                player.Session.Send(new RoomEntryInfoMessage
                {
                    RoomId = _room.Id,
                    Owner = isOwner
                });

                if (isOwner) player.Session.Send(new YouAreOwnerMessage());
            }

            avatarObject.Logic.AddStatus(RoomObjectAvatarStatus.FlatControl, ((int)controllerLevel).ToString());
        }

        public void KickPlayer(IRoomManipulator manipulator, int playerId)
        {
            if (playerId <= 0) return;

            var player = _playerManager.GetPlayerById(playerId);

            if (player == null || !CanKickPlayer(manipulator, player)) return;

            if (player.RoomObject == null || player.RoomObject.Room != _room) return;

            if (player.RoomObject.Logic is PlayerLogic playerLogic)
            {
                playerLogic.Kick();
            }
        }

        public async Task BanPlayerIdWithDuration(IRoomManipulator manipulator, int playerId, double durationMs)
        {
            if (playerId <= 0 || durationMs == 0.0 || Bans.ContainsKey(playerId)) return;

            var player = await _playerManager.GetOfflinePlayerById(playerId);

            if (player == null || !CanAdjustPlayerBan(manipulator, player, true)) return;

            var expiration = DateTime.Now.AddMilliseconds(durationMs);

            if (!await _roomBanRepository.BanPlayerIdAsync(_room.Id, player.Id, expiration)) return;

            Bans.Add(player.Id, expiration);

            if (player.RoomObject == null || player.RoomObject.Room != _room) return;

            if (player.RoomObject.Logic is PlayerLogic playerLogic)
            {
                playerLogic.Kick();
            }
        }

        public async Task AdjustRightsForPlayerId(IRoomManipulator manipulator, int playerId, bool flag)
        {
            if (manipulator == null || ((manipulator.Id != playerId) && !IsOwner(manipulator))) return;

            if (Rights.Contains(playerId) == flag) return;

            var player = await _playerManager.GetOfflinePlayerById(playerId);

            if (player == null) return;

            if (flag)
            {
                if (!await _roomRightRepository.GiveRightsToPlayerIdAsync(_room.Id, playerId)) return;

                Rights.Add(playerId);

                SendOwnersComposer(new FlatControllerAddedMessage
                {
                    RoomId = _room.Id,
                    PlayerId = playerId,
                    PlayerName = player.Name
                });
            }
            else
            {
                if (!await _roomRightRepository.RemoveRightsForPlayerIdAsync(_room.Id, playerId)) return;

                Rights.Remove(playerId);

                SendOwnersComposer(new FlatControllerRemovedMessage
                {
                    RoomId = _room.Id,
                    PlayerId = playerId
                });
            }

            if (player.RoomObject != null) RefreshControllerLevel(player.RoomObject);
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
            {
                if (avatarObject.RoomObjectHolder is IPlayer player)
                {
                    if (!IsOwner(player)) continue;

                    player.Session.Send(composer);
                }
            }
        }

        public void SendRightsComposer(IComposer composer)
        {
            foreach (var avatarObject in _room.RoomUserManager.AvatarObjects.RoomObjects.Values)
            {
                if (avatarObject.RoomObjectHolder is IPlayer player)
                {
                    if (GetControllerLevel(player) < RoomControllerLevel.Rights) continue;

                    player.Session.Send(composer);
                }
            }
        }

        public bool CanKickPlayer(IRoomManipulator manipulator, IPlayer player)
        {
            if (IsOwner(player)) return false;

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

        public bool CanAdjustPlayerBan(IRoomManipulator manipulator, IPlayer player, bool flag)
        {
            if (IsOwner(player)) return false;

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

        public bool CanAdjustPlayerMute(IRoomManipulator manipulator, IPlayer player, bool flag)
        {
            if (IsOwner(player)) return false;

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

        public bool CanAdjustPlayerRights(IRoomManipulator manipulator, IPlayer player, bool flag)
        {
            if (_room.IsGroupRoom || !IsOwner(manipulator) || IsOwner(player)) return false;

            if (flag)
            {
                if (GetControllerLevel(player) >= RoomControllerLevel.Rights) return false;
            }
            else
            {
                if (GetControllerLevel(player) != RoomControllerLevel.Rights) return false;
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
                bool canGroupDecorate = false;

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
                bool canGroupDecorate = false;

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

            if (manipulator is IPlayer player)
            {
                if (furniture is IRoomFloorFurniture floorFurniture)
                {
                    if (floorFurniture.PlayerId == manipulator.Id) return FurniturePickupType.SendToManipulator;
                }

                else if (furniture is IRoomWallFurniture wallFurniture)
                {
                    if (wallFurniture.PlayerId == manipulator.Id) return FurniturePickupType.SendToManipulator;
                }
            }

            if (manipulator.HasPermission("can_steal_furniture")) return FurniturePickupType.SendToManipulator;

            if (GetControllerLevel(manipulator) >= RoomControllerLevel.GroupAdmin) return FurniturePickupType.SendToOwner;

            return FurniturePickupType.None;
        }
    }
}
