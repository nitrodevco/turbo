using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IRoomObject CreateRoomObject(IRoom room, int id, string type, string logicType)
        {
            IRoomObjectLogic logic = _logicFactory.GetLogic(logicType);

            IRoomObject roomObject = new RoomObject(room, id, type);

            roomObject.SetLogic(logic);

            return roomObject;
        }
    }
}
