using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Rooms
{
    public interface IRoomContainer
    {
        public IRoom GetRoom(int id);
        public IRoom AddRoom(IRoom room);
        public void RemoveRoom(int id);
        public void RemoveAllRooms();
    }
}
