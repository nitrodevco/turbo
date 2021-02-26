using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Rooms.Managers
{
    public class RoomSecurityManager
    {
        private IRoom Room { get; set; }

        public RoomSecurityManager(IRoom room)
        {
            Room = room;
        }

        public void Init()
        {

        }

        public void Dispose()
        {

        }
    }
}
