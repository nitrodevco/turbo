using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;

namespace Turbo.Rooms.Object
{
    public class RoomObjectFactory : IRoomObjectFactory
    {
        private readonly IRoomObjectLogicFactory _logicFactory;

        public RoomObjectFactory(IRoomObjectLogicFactory logicFactory)
        {
            _logicFactory = logicFactory;
        }

        public IRoomObjectAvatar CreateAvatarObject(IRoom room, IRoomObjectContainer<IRoomObjectAvatar> roomObjectContainer, int id, string logicType = "")
        {
            if (logicType == null || logicType.Length == 0) return null;

            IRoomObjectAvatar roomObject = new RoomObjectAvatar(room, roomObjectContainer, id);

            roomObject.SetLogic((IMovingAvatarLogic)_logicFactory.Create(logicType));

            return roomObject;
        }

        public IRoomObjectFloor CreateFloorObject(IRoom room, IRoomObjectContainer<IRoomObjectFloor> roomObjectContainer, int id, string logicType = "")
        {
            if (logicType == null || logicType.Length == 0) return null;

            var roomObject = new RoomObjectFloor(room, roomObjectContainer, id);

            roomObject.SetLogic((IFurnitureFloorLogic)_logicFactory.Create(logicType));

            return roomObject;
        }

        public IRoomObjectWall CreateWallObject(IRoom room, IRoomObjectContainer<IRoomObjectWall> roomObjectContainer, int id, string logicType = "")
        {
            if (logicType == null || logicType.Length == 0) return null;

            var roomObject = new RoomObjectWall(room, roomObjectContainer, id);

            roomObject.SetLogic((IFurnitureWallLogic)_logicFactory.Create(logicType));

            return roomObject;
        }
    }
}
