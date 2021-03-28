using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Rooms.Cycles;

namespace Turbo.Rooms.Managers
{
    public class RoomCycleManager : IRoomCycleManager
    {
        private readonly IRoom _room;

        public List<ICyclable> _cycles;

        public RoomCycleManager(IRoom room)
        {
            _room = room;

            _cycles = new List<ICyclable>();

            _cycles.Add(new RoomUserStatusCycle(_room));
        }

        public async Task RunCycles() => _cycles.ForEach(async (x) => await x.Cycle());

        public async ValueTask DisposeAsync() => _cycles.Clear();
    }
}
