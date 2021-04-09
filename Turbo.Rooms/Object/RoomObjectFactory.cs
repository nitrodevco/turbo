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

        public IRoomObject Create(IRoom room, IRoomObjectContainer roomObjectContainer, int id, string type, string logicType)
        {
            IRoomObjectLogic logic = _logicFactory.Create(logicType);

            IRoomObject roomObject = new RoomObject(room, roomObjectContainer, id, type);

            roomObject.SetLogic(logic);

            return roomObject;
        }
    }
}
