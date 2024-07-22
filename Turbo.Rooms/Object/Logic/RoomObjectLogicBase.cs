using System.Threading.Tasks;
using Turbo.Core.Events;
using Turbo.Core.Game.Rooms.Object.Logic;

namespace Turbo.Rooms.Object.Logic;

public abstract class RoomObjectLogicBase : IRoomObjectLogic
{
    public ITurboEventHub EventHub { get; private set; }

    public virtual void Dispose()
    {
        CleanUp();
    }

    public virtual bool OnReady()
    {
        return true;
    }

    public virtual async Task Cycle()
    {
    }

    public void SetEventHub(ITurboEventHub eventHub)
    {
        EventHub = eventHub;
    }

    protected virtual void CleanUp()
    {
    }
}