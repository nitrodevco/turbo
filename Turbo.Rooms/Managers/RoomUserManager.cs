using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets.Messages;
using Turbo.Rooms.Object;
using Turbo.Rooms.Object.Logic.Avatar;

namespace Turbo.Rooms.Managers
{
    public class RoomUserManager : IRoomUserManager
    {
        private IRoom _room;

        private readonly IList<ISession> _roomObservers;
        private int _roomObjectCounter;

        public IDictionary<int, IRoomObject> RoomObjects { get; private set; }

        public RoomUserManager(IRoom room)
        {

            _roomObservers = new List<ISession>();
            _roomObjectCounter = -1;
            _room = room;

            RoomObjects = new Dictionary<int, IRoomObject>();
        }

        public async ValueTask InitAsync()
        {

        }

        public async ValueTask DisposeAsync()
        {

        }

        public IRoomObject GetRoomObject(int id)
        {
            if (id < 0) return null;

            IRoomObject roomObject;

            if (RoomObjects.TryGetValue(id, out roomObject))
            {
                return roomObject;
            }

            return null;
        }

        public IRoomObject GetRoomObjectByUserId(int userId)
        {
            return null;
        }

        public IRoomObject GetRoomObjectByUsername(string username)
        {
            return null;
        }

        public IRoomObject AddRoomObject(IRoomObject roomObject, IPoint location)
        {
            if (roomObject == null) return null;

            IRoomObject existingRoomObject = GetRoomObject(roomObject.Id);

            if(existingRoomObject != null)
            {
                roomObject.Dispose();

                return null;
            }

            if (roomObject.Logic is not MovingAvatarLogic avatarLogic) return null;

            roomObject.SetLocation(location);

            IRoomTile roomTile = avatarLogic.GetCurrentTile();

            if (roomTile != null) roomTile.AddRoomObject(roomObject);

            avatarLogic.InvokeCurrentLocation();

            roomObject.NeedsUpdate = false;

            RoomObjects.Add(roomObject.Id, roomObject);

            // here we are only sending this roomObject to the room
            // the session player is not watching the room yet
            // COMPOSER: SendComposer(RoomUserComposer(roomObject), RoomUserStatusComposer(roomObject))

            UpdateTotalUsers();

            return roomObject;
        }

        public IRoomObject CreateRoomObjectAndAssign(IRoomObjectHolder roomObjectHolder, IPoint location)
        {
            if (roomObjectHolder == null) return null;

            IRoomObject roomObject = new RoomObject(_room, _roomObjectCounter++, roomObjectHolder.Type);

            if (roomObject == null) return null;

            if (!roomObjectHolder.SetRoomObject(roomObject)) return null;

            return AddRoomObject(roomObject, location);
        }

        public void RemoveRoomObject(int id)
        {
            IRoomObject roomObject = GetRoomObject(id);

            if (roomObject == null) return;

            // send composer, RemoveRoomUser(roomObject.Id)

            RoomObjects.Remove(id);

            // if the room object was playing a game, remove it from that game

            UpdateTotalUsers();

            roomObject.Dispose();

            _room.TryDispose();
        }

        public void RemoveAllRoomObjects()
        {
            foreach (int id in RoomObjects.Keys) RemoveRoomObject(id);
        }

        public void EnterRoom(IPlayer player, IPoint location = null)
        {
            
        }

        private void UpdateTotalUsers()
        {

        }

        public void SendComposer(IComposer composer)
        {
            foreach(ISession session in _roomObservers)
            {
                session.Send(composer);
            }
        }
    }
}
