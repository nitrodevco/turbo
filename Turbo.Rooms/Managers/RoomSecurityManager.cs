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
using Turbo.Core.Storage;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Room;
using Turbo.Database.Repositories.Player;
using Turbo.Database.Repositories.Room;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Packets.Outgoing.Room.Permissions;
using Turbo.Packets.Outgoing.RoomSettings;

namespace Turbo.Rooms.Managers
{
    public class RoomSecurityManager : Component, IRoomSecurityManager
    {
        private readonly IRoom _room;
        private readonly IPlayerManager _playerManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IList<int> Rights { get; private set; }
        public IDictionary<int, DateTime> Bans { get; private set; }

        public RoomSecurityManager(
            IRoom room,
            IPlayerManager playerManager,
            IServiceScopeFactory serviceScopeFactory)
        {
            _room = room;
            _playerManager = playerManager;
            _serviceScopeFactory = serviceScopeFactory;

            Rights = new List<int>();
            Bans = new ConcurrentDictionary<int, DateTime>();
        }

        protected override async Task OnInit()
        {
            await LoadRights();
            await LoadBans();
        }

        protected override async Task OnDispose()
        {
            Rights.Clear();
            Bans.Clear();
        }

        public bool IsStrictOwner(IRoomManipulator manipulator)
        {
            if (_room.RoomDetails.PlayerId == manipulator.Id) return true;

            return false;
        }

        public bool IsOwner(IRoomManipulator manipulator)
        {
            if (IsStrictOwner(manipulator)) return true;

            if (manipulator.HasPermission("any_room_owner")) return true;

            return false;
        }

        public bool IsPlayerRoomBanned(IPlayer player)
        {
            if (player == null || !Bans.ContainsKey(player.Id)) return false;

            if (Bans.TryGetValue(player.Id, out var banExpiration))
            {
                if (banExpiration == null) return false;

                if (DateTime.Compare(DateTime.Now, banExpiration) < 0) return true;

                Bans.Remove(player.Id);
            }

            return false;
        }

        public RoomControllerLevel GetControllerLevel(IRoomManipulator manipulator)
        {
            if (IsOwner(manipulator)) return RoomControllerLevel.Moderator;

            bool isGroup = false;

            if (isGroup)
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

            return RoomControllerLevel.None;
        }

        public void RefreshControllerLevel(IRoomObjectAvatar avatarObject)
        {
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

        public async Task AdjustRightsForPlayerId(IRoomManipulator manipulator, int playerId, bool flag)
        {
            if (manipulator == null || ((manipulator.Id != playerId) && !IsOwner(manipulator))) return;

            if (flag)
            {
                if (Rights.Contains(playerId)) return;

                var player = await _playerManager.GetOfflinePlayerById(playerId);

                if (player == null) return;

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var roomRightRepository = scope.ServiceProvider.GetService<IRoomRightRepository>();

                    if (!await roomRightRepository.GiveRightsToPlayerIdAsync(_room.Id, playerId)) return;

                    Rights.Add(playerId);
                }

                if (player.RoomObject != null) RefreshControllerLevel(player.RoomObject);

                SendOwnersComposer(new FlatControllerAddedMessage
                {
                    RoomId = _room.Id,
                    PlayerId = playerId,
                    PlayerName = player.Name
                });
            }
            else
            {
                if (!Rights.Contains(playerId)) return;

                var player = await _playerManager.GetOfflinePlayerById(playerId);

                if (player == null) return;

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var roomRightRepository = scope.ServiceProvider.GetService<IRoomRightRepository>();

                    if (!await roomRightRepository.RemoveRightsForPlayerIdAsync(_room.Id, playerId)) return;

                    Rights.Remove(playerId);
                }

                if (player.RoomObject != null) RefreshControllerLevel(player.RoomObject);

                SendOwnersComposer(new FlatControllerRemovedMessage
                {
                    RoomId = _room.Id,
                    PlayerId = playerId
                });
            }
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

        private async Task LoadRights()
        {
            Rights.Clear();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var roomRightRepository = scope.ServiceProvider.GetService<IRoomRightRepository>();
                var entities = await roomRightRepository.FindAllByRoomIdAsync(_room.Id);

                foreach (var entity in entities)
                {
                    Rights.Add(entity.PlayerEntityId);
                }
            }
        }

        private async Task LoadBans()
        {
            Bans.Clear();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var roomBanRepository = scope.ServiceProvider.GetService<IRoomBanRepository>();
                var entities = await roomBanRepository.FindAllByRoomIdAsync(_room.Id);

                foreach (var entity in entities)
                {
                    if (DateTime.Compare(DateTime.Now, entity.DateExpires) >= 0)
                    {
                        await roomBanRepository.RemoveBanEntityAsync(entity);

                        continue;
                    }

                    Bans.Add(entity.PlayerEntityId, entity.DateExpires);
                }
            }
        }

        public bool CanManipulateFurniture(IRoomManipulator manipulator, IRoomFurniture furniture)
        {
            if (furniture == null) return false;

            if (manipulator == null) return true;

            var controllerLevel = GetControllerLevel(manipulator);

            if (controllerLevel >= RoomControllerLevel.GroupAdmin) return true;

            bool isGroup = false;
            bool canGroupDecorate = false;

            if (isGroup)
            {
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

            bool isGroup = false;
            bool canGroupDecorate = false;

            if (isGroup)
            {
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
