using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectFurnitureHolder : IRoomObjectHolder
    {
        public Task<bool> SetupRoomObject();
        public string LogicType { get; }
        public string PlayerName { get; set; }
        public int PlayerId { get; }
    }
}
