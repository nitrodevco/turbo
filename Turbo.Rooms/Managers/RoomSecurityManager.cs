using System.Collections.Generic;
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
using Turbo.Database.Repositories.Room;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Packets.Outgoing.Room.Permissions;

namespace Turbo.Rooms.Managers
{
    public class RoomSecurityManager : Component, IRoomSecurityManager
    {
        private readonly IRoom _room;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IList<int> Rights { get; private set; }

        public RoomSecurityManager(
            IRoom room,
            IServiceScopeFactory serviceScopeFactory)
        {
            _room = room;
            _serviceScopeFactory = serviceScopeFactory;

            Rights = new List<int>();
        }

        protected override async Task OnInit()
        {
            await LoadRights();
        }

        protected override async Task OnDispose()
        {
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

                foreach (RoomRightEntity roomRightEntity in entities)
                {
                    Rights.Add(roomRightEntity.PlayerEntityId);
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
