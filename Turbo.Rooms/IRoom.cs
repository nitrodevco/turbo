using System;
using Turbo.Core;
using Turbo.Rooms.Mapping;

namespace Turbo.Rooms
{
    public interface IRoom : IAsyncInitialisable, IAsyncDisposable
    {
        public int Id { get; }
        public RoomDetails RoomDetails { get; }

        public IRoomModel RoomModel { get; }

        public void TryDispose();
        public void CancelDispose();
    }
}
