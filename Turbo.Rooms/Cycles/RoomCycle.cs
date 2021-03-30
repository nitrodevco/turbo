using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms;

namespace Turbo.Rooms.Cycles
{
    public abstract class RoomCycle : ICyclable
    {
        protected readonly IRoom _room;

        public RoomCycle(IRoom room)
        {
            _room = room;
        }

        public abstract Task Cycle();
    }
}
