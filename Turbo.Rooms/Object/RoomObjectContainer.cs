using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object
{
    public class RoomObjectContainer<T> : IRoomObjectContainer<T> where T : IRoomObject
    {
        public ConcurrentDictionary<int, T> RoomObjects { get; private set; }

        private readonly Action<T> _onRemove;
        private int _counter;

        public RoomObjectContainer(Action<T> onRemove)
        {
            RoomObjects = new ConcurrentDictionary<int, T>();
            _onRemove = onRemove;
        }

        public bool AddRoomObject(T roomObject)
        {
            if (roomObject == null) return false;

            return RoomObjects.TryAdd(roomObject.Id, roomObject);
        }

        public T GetRoomObject(int id)
        {
            if ((id >= 0) && RoomObjects.TryGetValue(id, out T roomObject))
            {
                return roomObject;
            }

            return default(T);
        }

        public void RemoveRoomObject(params int[] ids)
        {
            foreach (int id in ids)
            {
                RemoveRoomObject(GetRoomObject(id));
            }
        }

        public void RemoveRoomObject(params T[] roomObjects)
        {
            foreach (T roomObject in roomObjects)
            {
                if (roomObject == null) continue;

                if (!RoomObjects.TryRemove(new KeyValuePair<int, T>(roomObject.Id, roomObject))) continue;

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