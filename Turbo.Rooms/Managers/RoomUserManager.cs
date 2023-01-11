using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;
using Turbo.Packets.Outgoing.Room.Action;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object.Logic.Avatar;
using Turbo.Rooms.Object;

namespace Turbo.Rooms.Managers
{
    public class RoomUserManager : IRoomUserManager
    {
        private readonly IRoom _room;
        private readonly IRoomObjectFactory _roomObjectFactory;

        public IRoomObjectContainer<IRoomObjectAvatar> AvatarObjects { get; private set; }

        public RoomUserManager(
            IRoom room,
            IRoomObjectFactory roomObjectFactory)
        {
            _room = room;
            _roomObjectFactory = roomObjectFactory;

            AvatarObjects = new RoomObjectContainer<IRoomObjectAvatar>();
        }

        public async ValueTask InitAsync()
        {

        }

        public async ValueTask DisposeAsync()
        {
            RemoveAllRoomObjects();
        }

        public IRoomObjectAvatar GetRoomObjectByUserId(int userId)
        {
            return null;
        }

        public IRoomObjectAvatar GetRoomObjectByUsername(string username)
        {
            return null;
        }

        public IRoomObjectAvatar AddRoomObject(IRoomObjectAvatar avatarObject, IPoint location = null)
        {
            if (avatarObject == null) return null;

            var existingRoomObject = AvatarObjects.GetRoomObject(avatarObject.Id);

            if (existingRoomObject != null)
            {
                avatarObject.Dispose();

                return null;
            }

            if (location == null) location = _room.RoomModel.DoorLocation.Clone();

            avatarObject.SetLocation(location);
            avatarObject.Location.SetRotation(location.Rotation);

            if (!avatarObject.Logic.OnReady())
            {
                avatarObject.Dispose();

                return null;
            }

            _room.RoomMap.AddRoomObjects(avatarObject);

            AvatarObjects.AddRoomObject(avatarObject);

            UpdateTotalUsers();

            avatarObject.Logic.CanWalk = true;

            return avatarObject;
        }

        public IRoomObjectAvatar CreateRoomObjectAndAssign(IRoomObjectAvatarHolder userHolder, IPoint location = null)
        {
            if (userHolder == null) return null;

            var logicType = "";

            switch (userHolder.Type)
            {
                case RoomObjectHolderType.User:
                    logicType = "user";
                    break;
                case RoomObjectHolderType.Pet:
                    logicType = "pet";
                    break;
                case RoomObjectHolderType.Bot:
                    logicType = "bot";
                    break;
                case RoomObjectHolderType.RentableBot:
                    logicType = "rentablebot";
                    break;
            }

            var roomObject = _roomObjectFactory.CreateAvatarObject(_room, AvatarObjects, AvatarObjects.GetNextId(), logicType);

            if (roomObject == null) return null;

            if (!userHolder.SetRoomObject(roomObject)) return null;

            return AddRoomObject(roomObject, location);
        }

        public void RemoveRoomObject(params IRoomObjectAvatar[] avatarObjects)
        {
            foreach (var avatarObject in avatarObjects)
            {
                RemoveRoomObject(avatarObject.Id);
            }
        }

        public void RemoveRoomObject(params int[] ids)
        {
            foreach (int id in ids)
            {
                RemoveRoomObject(id);
            }
        }

        public void RemoveRoomObject(int objectId)
        {
            var roomObject = AvatarObjects.GetRoomObject(objectId);

            if (roomObject == null) return;

            _room.RoomMap.RemoveRoomObjects(null, roomObject);

            AvatarObjects.RemoveRoomObject(objectId);

            // if the room object was playing a game, remove it from that game

            roomObject.Dispose();

            UpdateTotalUsers();

            _room.TryDispose();
        }

        public void RemoveAllRoomObjects()
        {
            foreach (var objectId in AvatarObjects.RoomObjects.Keys) RemoveRoomObject(objectId);
        }

        public IRoomObjectAvatar EnterRoom(IPlayer player, IPoint location = null)
        {
            if (player == null) return null;

            var avatarObject = CreateRoomObjectAndAssign(player, location);

            List<IRoomObjectAvatar> roomObjects = new();
            List<IComposer> composers = new();

            foreach (var existingAvatarObject in AvatarObjects.RoomObjects.Values)
            {
                roomObjects.Add(existingAvatarObject);

                if (existingAvatarObject.Logic is AvatarLogic avatarLogic)
                {
                    var danceType = avatarLogic.DanceType;
                    var isIdle = avatarLogic.IsIdle;

                    if (danceType > RoomObjectAvatarDanceType.None)
                    {
                        composers.Add(new DanceMessage
                        {
                            ObjectId = existingAvatarObject.Id,
                            DanceStyle = (int)danceType
                        });
                    }

                    if (isIdle)
                    {
                        composers.Add(new SleepMessage
                        {
                            ObjectId = existingAvatarObject.Id,
                            Sleeping = isIdle
                        });
                    }
                }
            }

            player.Session.SendQueue(new UsersMessage
            {
                RoomObjects = roomObjects
            });

            player.Session.SendQueue(new UserUpdateMessage
            {
                RoomObjects = roomObjects
            });

            foreach (IComposer composer in composers) player.Session.SendQueue(composer);

            player.Session.Flush();

            return avatarObject;
        }

        private void UpdateTotalUsers()
        {
            int totalUsers = 0;

            foreach (var roomObject in AvatarObjects.RoomObjects.Values)
            {
                if (roomObject.RoomObjectHolder is IPlayer) continue;

                totalUsers++;
            }

            _room.RoomDetails.UsersNow = totalUsers;
        }

        public void SendComposer(IComposer composer)
        {
            _room.SendComposer(composer);
        }
    }
}
