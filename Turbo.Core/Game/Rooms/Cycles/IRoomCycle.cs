using System;

namespace Turbo.Core.Game.Rooms.Cycles;

public interface IRoomCycle : IDisposable
{
    public void Run();
}