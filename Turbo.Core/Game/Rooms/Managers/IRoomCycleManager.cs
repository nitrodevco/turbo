using System;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomCycleManager : IDisposable, ICyclable
    {
        public void Start();
        public void Stop();
    }
}
