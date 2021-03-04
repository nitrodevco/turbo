using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core;

namespace Turbo.Rooms
{
    public interface IRoom : IAsyncInitialisable, IAsyncDisposable
    {
        public int Id { get; }
        public IRoomManager RoomManager { get; }
        public RoomDetails RoomDetails { get; }

        public void TryDispose();
        public void CancelDispose();
    }
}
