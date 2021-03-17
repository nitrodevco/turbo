using System;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;

namespace Turbo.Core.Game.Rooms
{
    public interface IRoom : IAsyncInitialisable, IAsyncDisposable, ICyclable
    {
        public IRoomManager RoomManager { get; }
        public IRoomDetails RoomDetails { get; }
        public IRoomModel RoomModel { get; }
        public IRoomMap RoomMap { get; }

        public IRoomSecurityManager RoomSecurityManager { get; }
        public IRoomFurnitureManager RoomFurnitureManager { get; }
        public IRoomUserManager RoomUserManager { get; }

        public void TryDispose();
        public void CancelDispose();

        public int Id { get; }
    }
}
