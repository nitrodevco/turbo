using System;
using Turbo.Core.Game.Rooms.Mapping;

namespace Turbo.Core.Game.Rooms
{
    public interface IRoom : IAsyncInitialisable, IAsyncDisposable, ICyclable
    {
        public int Id { get; }
        public IRoomDetails RoomDetails { get; }

        public IRoomModel RoomModel { get; }
        public IRoomMap RoomMap { get; }

        public void TryDispose();
        public void CancelDispose();
    }
}
