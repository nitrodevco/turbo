using System;
using System.Collections.Generic;
using Turbo.Rooms.Mapping;

namespace Turbo.Rooms
{
    public class RoomManager : IRoomManager
    {
        private Dictionary<int, IRoom> Rooms { get; set; }
        private Dictionary<string, IRoomModel> Models { get; set; }

        public RoomManager()
        {
            Rooms = new Dictionary<int, IRoom>();
            Models = new Dictionary<string, IRoomModel>();
        }

        public void OnInit()
        {
            LoadModels();

            // set a interval for TryDisposeAllRooms()
        }

        public void OnDispose()
        {
            // cancel the dispose interval

            RemoveAllRooms();
        }

        public IRoom GetRoom(int id)
        {
            IRoom room = GetOnlineRoom(id);

            if (room != null) return room;

            return GetOfflineRoom(id);
        }

        public IRoom GetOnlineRoom(int id)
        {
            IRoom room = Rooms[id];

            if (room == null) return null;

            room.CancelDispose();

            return room;
        }

        public IRoom GetOfflineRoom(int id)
        {
            IRoom room = GetOnlineRoom(id);

            if (room != null) return room;

            // room details should be populated from the RoomRepository where roomId = id
            
            RoomDetails roomDetails = new RoomDetails();

            room = new Room(this, roomDetails);

            return AddRoom(room);
        }

        public IRoom AddRoom(IRoom room)
        {
            if (room == null) return null;

            IRoom existing = GetOnlineRoom(room.Id);

            if(existing != null)
            {
                if (room != existing) room.Dispose();

                return existing;
            }

            Rooms.Add(room.Id, room);

            return room;
        }

        public void RemoveRoom(int id)
        {
            IRoom room = GetOnlineRoom(id);

            if (room == null) return;

            Rooms.Remove(room.Id);

            room.Dispose();
        }

        public void RemoveAllRooms()
        {
            if (Rooms.Count == 0) return;

            foreach(int id in Rooms.Keys)
            {
                Rooms.Remove(id);

                RemoveRoom(id);
            }
        }

        public void TryDisposeAllRooms()
        {
            foreach(var room in Rooms.Values)
            {
                room.TryDispose();
            }
        }



        private void LoadModels()
        {

        }
    }
}
