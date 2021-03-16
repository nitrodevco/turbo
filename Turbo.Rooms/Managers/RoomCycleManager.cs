using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Cycles;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Rooms.Cycles;

namespace Turbo.Rooms.Managers
{
    public class RoomCycleManager : IRoomCycleManager
    {
        private readonly IRoom _room;

        public IList<IRoomCycle> _tasks;

        public RoomCycleManager(IRoom room)
        {
            _room = room;

            _tasks.Add(new RoomUserStatusCycle(_room));
        }

        public void RunCycles()
        {
            foreach (IRoomCycle roomTask in _tasks) roomTask.Run();
        }

        public void Dispose()
        {
            foreach(IRoomCycle roomTask in _tasks)
            {
                roomTask.Dispose();
            }

            _tasks.Clear();
        }
    }
}
