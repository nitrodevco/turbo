using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Rooms.Object;

namespace Turbo.Rooms.Managers
{
    public class RoomFurnitureManager
    {
        private IRoom Room { get; set; }

        private IRoomObjectManager _roomObjectManager { get; set; }

        public RoomFurnitureManager(IRoom room)
        {
            Room = room;

            _roomObjectManager = new RoomObjectManager();
        }

        public void Init()
        {

        }

        public void Dispose()
        {

        }
    }
}
