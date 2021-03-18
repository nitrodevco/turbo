using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Packets.Messages;

namespace Turbo.Core.Game.Rooms
{
    public interface IRoom : IAsyncInitialisable, IAsyncDisposable, ICyclable
    {
        public IRoomManager RoomManager { get; }
        public ILogger<IRoom> Logger { get; }
        public IRoomDetails RoomDetails { get; }
        public IRoomModel RoomModel { get; }
        public IRoomMap RoomMap { get; }
        public IRoomSecurityManager RoomSecurityManager { get; }
        public IRoomFurnitureManager RoomFurnitureManager { get; }
        public IRoomUserManager RoomUserManager { get; }

        public void TryDispose();
        public void CancelDispose();

        public void EnterRoom(IPlayer player);
        public void SendComposer(IComposer composer);

        public int Id { get; }
    }
}
