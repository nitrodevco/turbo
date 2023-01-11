using System.Collections.Generic;

namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectContainer<T> where T : IRoomObject
    {
        public IDictionary<int, T> RoomObjects { get; }

        public bool AddRoomObject(T roomObject);
        public T GetRoomObject(int id);
        public void RemoveRoomObject(params int[] ids);
        public void RemoveRoomObject(params T[] roomObjects);
        public void RemoveAllRoomObjects();
        public int GetNextId();
    }
}
