using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Tasks
{
    public class RoomUserStatusTask
    {
        private readonly IRoom _room;

        public RoomUserStatusTask(IRoom room)
        {
            _room = room;
        }

        public void Run()
        {
            IList<IRoomObject> updatedObjects = new List<IRoomObject>();

            IList<IRoomObject> roomObjects = _room.
        }
    }
}
