using System;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomCycleManager : IAsyncDisposable
    {
        public Task RunCycles();
    }
}
