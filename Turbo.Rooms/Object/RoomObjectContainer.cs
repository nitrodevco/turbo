using System.Collections.Generic;
using System.Linq;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object
{
    public class RoomObjectContainer<T> : IRoomObjectContainer<T> where T : IRoomObject
    {
        public IDictionary<int, T> RoomObjects { get; private set; }

        private int _counter;

        public RoomObjectContainer()
        {
            RoomObjects = new Dictionary<int, T>();
        }

        public bool AddRoomObject(T roomObject)
        {
            if (roomObject == null) return false;

            if (RoomObjects.Keys.Contains(roomObject.Id)) return false;

            if (RoomObjects.Values.Contains(roomObject)) return false;

            RoomObjects.Add(roomObject.Id, roomObject);

            return true;
        }

        public T GetRoomObject(int id)
        {
            if (id < 0) return default(T);

            if (RoomObjects.TryGetValue(id, out T roomObject))
            {
                return roomObject;
            }

            return default(T);
        }

        public void RemoveRoomObject(params int[] ids)
        {
            if (ids.Count() == 1)
            {
                RoomObjects.Remove(ids[0]);

                return;
            }

            foreach (int id in ids)
            {
                RoomObjects.Remove(id);
            }
        }

        public void RemoveRoomObject(params T[] roomObjects)
        {
            if (roomObjects.Count() == 1)
            {
                RoomObjects.Remove(roomObjects[0].Id);

                return;
            }

            foreach (T roomObject in roomObjects)
            {
                RoomObjects.Remove(roomObject.Id);
            }
        }

        public void RemoveAllRoomObjects()
        {
            RemoveRoomObject(RoomObjects.Keys.ToArray());
        }

        public int GetNextId() => ++_counter;
    }
}