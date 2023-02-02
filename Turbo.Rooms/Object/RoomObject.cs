using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Object.Logic;

namespace Turbo.Rooms.Object
{
    public abstract class RoomObject : IRoomObject
    {
        public IRoom Room { get; private set; }
        public int Id { get; private set; }
        public bool NeedsUpdate { get; set; }

        protected bool _isDisposing;

        public RoomObject(IRoom room, int id)
        {
            Room = room;

            Id = id;
        }

        public virtual void Dispose()
        {
            if (Disposed || _isDisposing) return;

            _isDisposing = true;

            OnDispose();

            Id = -1;
            Room = null;
            NeedsUpdate = false;
            _isDisposing = false;
        }

        protected abstract void OnDispose();

        public bool Disposed => (Room == null);
    }
}
