using System;

namespace Turbo.Core.Game.Rooms.Managers;

public interface IRoomCycleManager : IDisposable, ICyclable
{
    public void AddCycle(ICyclable cycle);
    public void RemoveCycle(ICyclable cycle);
    public void Start();
    public void Stop();
}