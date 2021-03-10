using System;
using System.Collections.Generic;

namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectManager : IDisposable
    {
        public IDictionary<int, IRoomObject> RoomObjects { get; }
        public IDictionary<string, IDictionary<int, IRoomObject>> RoomObjectsPerType { get; }

        public IRoomObject GetRoomObject(int id);
        public IRoomObject CreateRoomObject(int id, string type);
        public void RemoveRoomObject(int id);
        public void RemoveAllRoomObjects();
    }
}
