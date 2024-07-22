using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;

namespace Turbo.Rooms.Managers;

public class RoomCycleManager(IRoom _room) : IRoomCycleManager
{
    public List<ICyclable> _cycles = [];

    private bool _running;

    public void Start()
    {
        _running = true;
    }

    public void Stop()
    {
        _running = false;
    }

    public void Dispose()
    {
        _cycles.Clear();
    }

    public void AddCycle(ICyclable cycle)
    {
        if (cycle == null || _cycles.Contains(cycle)) return;

        _cycles.Add(cycle);
    }

    public void RemoveCycle(ICyclable cycle)
    {
        if (cycle == null || !_cycles.Contains(cycle)) return;

        _cycles.Remove(cycle);
    }

    public async Task Cycle()
    {
        if (!_running) return;

        _cycles.ForEach(async x => await x.Cycle());
    }
}