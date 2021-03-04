using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async ValueTask InitAsync()
        {
            await LoadModels();

            // set a interval for TryDisposeAllRooms()
        }

        public async ValueTask DisposeAsync()
        {
            // cancel the TryDisposeAllRooms interval

            await RemoveAllRooms();
        }

        public async Task<IRoom> GetRoom(int id)
        {
            IRoom room = GetOnlineRoom(id);

            if (room != null) return room;

            return await GetOfflineRoom(id);
        }

        public IRoom GetOnlineRoom(int id)
        {
            IRoom room = Rooms[id];

            if (room == null) return null;

            room.CancelDispose();

            return room;
        }

        public async Task<IRoom> GetOfflineRoom(int id)
        {
            IRoom room = GetOnlineRoom(id);

            if (room != null) return room;

            // room details should be populated from the RoomRepository where roomId = id
            
            RoomDetails roomDetails = new RoomDetails();

            room = new Room(this, roomDetails);

            return await AddRoom(room);
        }

        public async Task<IRoom> AddRoom(IRoom room)
        {
            if (room == null) return null;

            IRoom existing = GetOnlineRoom(room.Id);

            if(existing != null)
            {
                if (room != existing) await room.DisposeAsync();

                return existing;
            }

            Rooms.Add(room.Id, room);

            return room;
        }

        public async ValueTask RemoveRoom(int id)
        {
            IRoom room = GetOnlineRoom(id);

            if (room == null) return;

            Rooms.Remove(room.Id);

            await room.DisposeAsync();
        }

        public async ValueTask RemoveAllRooms()
        {
            if (Rooms.Count == 0) return;

            foreach(int id in Rooms.Keys)
            {
                await RemoveRoom(id);
            }
        }

        public void TryDisposeAllRooms()
        {
            foreach(var room in Rooms.Values)
            {
                room.TryDispose();
            }
        }

        private async ValueTask LoadModels()
        {

        }
    }
}
