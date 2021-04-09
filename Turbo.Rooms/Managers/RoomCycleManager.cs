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

        private bool _running;

        public RoomCycleManager(IRoom room)
        {
            _room = room;

            _cycles = new List<ICyclable>();

            _cycles.Add(new RoomObjectCycle(_room));
            _cycles.Add(new RoomRollerCycle(_room));
            _cycles.Add(new RoomUserStatusCycle(_room));
        }

        public void Start()
        {
            _running = true;
        }

        public void Stop()
        {
            _running = false;
        }

        public void Dispose() => _cycles.Clear();

        public async Task Cycle()
        {
            if (!_running) return;

            _cycles.ForEach(async (x) => await x.Cycle());
        }
    }
}
