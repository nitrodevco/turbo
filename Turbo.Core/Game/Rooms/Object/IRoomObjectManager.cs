using System;
using System.Collections.Generic;

namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectManager : IDisposable
    {
        public Dictionary<int, IRoomObject> RoomObjects { get; }
        public Dictionary<string, Dictionary<int, IRoomObject>> RoomObjectsPerType { get; }

        public IRoomObject GetRoomObject(int id);
        public IRoomObject CreateRoomObject(int id, string type);
        public void RemoveRoomObject(int id);
        public void RemoveAllRoomObjects();
    }
}
