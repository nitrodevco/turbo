using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object;

public abstract class RoomObject : IRoomObject
{
    protected bool _isDisposing;

    public RoomObject(IRoom room, int id)
    {
        Room = room;

        Id = id;
    }

    public IRoom Room { get; private set; }
    public int Id { get; private set; }
    public bool NeedsUpdate { get; set; }

    public virtual void Dispose()
    {
        if (Disposed || _isDisposing) return;

        _isDisposing = true;

        OnDispose();

        Room = null;
        Id = -1;
        NeedsUpdate = false;
        _isDisposing = false;
    }

    public bool Disposed => Room == null;

    protected abstract void OnDispose();
}