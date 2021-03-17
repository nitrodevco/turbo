using System.Collections.Generic;

namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectContainer
    {
        public IDictionary<int, IRoomObject> RoomObjects { get; }

        public IRoomObject GetRoomObject(int id);
        public void RemoveRoomObject(int id);
        public void RemoveAllRoomObjects();
    }
}
