using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;
using Turbo.Rooms.Object;

namespace Turbo.Rooms.Managers
{
    public class RoomUserManager : IRoomUserManager
    {
        private readonly IRoom _room;

        private readonly IList<IPlayer> _roomObservers;
        private int _roomObjectCounter;

        public IDictionary<int, IRoomObject> RoomObjects { get; private set; }

        public RoomUserManager(IRoom room)
        {
            _room = room;

            _roomObservers = new List<IPlayer>();
            _roomObjectCounter = -1;

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

            try
            {
                IRoomObject roomObject;

                if (RoomObjects.TryGetValue(id, out roomObject)) return roomObject;

                return null;
            }

            catch(Exception e)
            {
                return null;
            }
        }

        public IRoomObject GetRoomObjectByUserId(int userId)
        {
            return null;
        }

        public IRoomObject GetRoomObjectByUsername(string username)
        {
            return null;
        }

        public IRoomObject AddRoomObject(IRoomObject roomObject, IPoint location, IPoint direction)
        {
            if (roomObject == null) return null;

            IRoomObject existingRoomObject = GetRoomObject(roomObject.Id);

            if(existingRoomObject != null)
            {
                roomObject.Dispose();

                return null;
            }

            roomObject.SetLocation(location);
            roomObject.SetDirection(direction);

            IRoomTile roomTile = _room.RoomMap.GetTile(roomObject.Location);

            if(roomTile != null)
            {
                roomTile.AddUser(roomObject);
            }

            // invoke the users location

            // COMPOSER: SendComposer(RoomUserComposer(roomObject), RoomUserStatusComposer(roomObject))

            UpdateTotalUsers();

            return roomObject;
        }

        public IRoomObject CreateRoomObjectAndAssign(IRoomObjectHolder roomObjectHolder, IPoint location, IPoint direction)
        {
            if (roomObjectHolder == null) return null;

            IRoomObject roomObject = new RoomObject(_roomObjectCounter++, roomObjectHolder.Type);

            if (roomObject == null) return null;

            if (!roomObjectHolder.SetRoomObject(roomObject)) return null;

            return AddRoomObject(roomObject, location, direction);
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

        public void EnterRoom()
        {

        }

        private void UpdateTotalUsers()
        {

        }

        public void SendComposer(IComposer composer)
        {
            if (composer == null) return;

            foreach(IPlayer sessionPlayer in _roomObservers)
            {
                // send packet
            }
        }
    }
}
