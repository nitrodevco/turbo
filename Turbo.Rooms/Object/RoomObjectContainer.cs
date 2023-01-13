using System;
using System.Collections.Generic;
using System.Linq;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object
{
    public class RoomObjectContainer<T> : IRoomObjectContainer<T> where T : IRoomObject
    {
        public IDictionary<int, T> RoomObjects { get; private set; }

        private readonly Action<T> _onRemove;
        private int _counter;

        public RoomObjectContainer(Action<T> onRemove)
        {
            RoomObjects = new Dictionary<int, T>();
            _onRemove = onRemove;
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
            foreach (int id in ids)
            {
                RemoveRoomObject(RoomObjects[id]);
            }
        }

        public void RemoveRoomObject(params T[] roomObjects)
        {
            foreach (T roomObject in roomObjects)
            {
                var existingObject = GetRoomObject(roomObject.Id);

                if (existingObject == null) continue;

                RoomObjects.Remove(roomObject.Id);

                _onRemove(roomObject);
            }
        }

        public void RemoveAllRoomObjects()
        {
            RemoveRoomObject(RoomObjects.Values.ToArray());
        }

        public int GetNextId() => ++_counter;
    }
}