using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Rooms.Mapping;

namespace Turbo.Rooms
{
    public interface IRoom : IAsyncInitialisable, IAsyncDisposable
    {
        public int Id { get; }
        public RoomDetails RoomDetails { get; }

        public IRoomModel RoomModel { get; }
        public IRoomMap RoomMap { get; }

        public void TryDispose();
        public void CancelDispose();
    }
}
