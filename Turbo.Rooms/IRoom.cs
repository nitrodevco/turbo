using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Rooms
{
    public interface IRoom
    {
        public int Id { get; }
        public IRoomManager RoomManager { get; }
        public RoomDetails RoomDetails { get; }

        public void Dispose();
        public void TryDispose();
        public void CancelDispose();
    }
}
