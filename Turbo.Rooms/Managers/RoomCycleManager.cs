using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Cycles;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Rooms.Cycles;

namespace Turbo.Rooms.Managers
{
    public class RoomCycleManager : IRoomCycleManager
    {
        private readonly IRoom _room;

        public IList<ICyclable> _cycles;

        public RoomCycleManager(IRoom room)
        {
            _room = room;

            _cycles.Add(new RoomUserStatusCycle(_room));
        }

        public async Task RunCycles()
        {
            foreach (ICyclable cycle in _cycles) await cycle.Cycle();
        }

        public async ValueTask DisposeAsync()
        {
            _cycles.Clear();
        }
    }
}
