using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Packets.Messages;
using Turbo.Database.Entities.Room;
using Turbo.Database.Repositories.Room;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Packets.Outgoing.Room.Permissions;

namespace Turbo.Rooms.Managers
{
    public class RoomSecurityManager : IRoomSecurityManager
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

        public async ValueTask InitAsync()
        {
            await LoadRights();
        }

        public async ValueTask DisposeAsync()
        {

        }

        public bool IsOwner(IRoomManipulator manipulator)
        {
            if (IsStrictOwner(manipulator)) return true;

            if (manipulator.HasPermission("any_room_owner")) return true;

            return false;
        }

        public bool IsStrictOwner(IRoomManipulator manipulator)
        {
            if (_room.RoomDetails.PlayerId == manipulator.Id) return true;

            return false;
        }

        public bool IsController(IRoomManipulator manipulator)
        {
            if (IsOwner(manipulator)) return true;

            if (manipulator.HasPermission("any_room_rights")) return true;

            if (Rights.Contains(manipulator.Id)) return true;

            return false;
        }

        public void RefreshControllerLevel(IRoomObject roomObject)
        {
            bool isOwner = false;
            RoomControllerLevel controllerLevel = RoomControllerLevel.None;

            if(roomObject.RoomObjectHolder is IPlayer player)
            {
                if(IsOwner(player))
                {
                    isOwner = true;
                    controllerLevel = RoomControllerLevel.Moderator;
                }

                else if(IsController(player))
                {
                    controllerLevel = RoomControllerLevel.Rights;
                }

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

                if(isOwner)
                {
                    player.Session.Send(new YouAreOwnerMessage());
                }
            }

            if(roomObject.Logic is IMovingAvatarLogic avatarLogic)
            {
                avatarLogic.AddStatus(RoomObjectAvatarStatus.FlatControl, ((int)controllerLevel).ToString());
            }
        }

        public void SendOwnersComposer(IComposer composer)
        {
            foreach(IRoomObject roomObject in _room.RoomUserManager.RoomObjects.Values)
            {
                if(roomObject.RoomObjectHolder is IPlayer player)
                {
                    if (!IsOwner(player)) continue;

                    player.Session.Send(composer);
                }
            }
        }

        public void SendRightsComposer(IComposer composer)
        {
            foreach (IRoomObject roomObject in _room.RoomUserManager.RoomObjects.Values)
            {
                if (roomObject.RoomObjectHolder is IPlayer player)
                {
                    if (!IsController(player)) continue;

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
    }
}
