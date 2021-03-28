using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Furniture
{
    public interface IFurniture : IRoomObjectFurnitureHolder, IDisposable
    {
        public bool SetRoom(IRoom room);
    }
}
