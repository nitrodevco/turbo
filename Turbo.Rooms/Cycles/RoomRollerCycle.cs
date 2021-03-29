using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms;

namespace Turbo.Rooms.Cycles
{
    public class RoomRollerCycle : RoomCycle
    {
        public RoomRollerCycle(IRoom room) : base(room)
        {

        }

        public override Task Cycle()
        {
            return Task.CompletedTask;
        }
    }
}
