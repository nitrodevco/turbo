using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectFurnitureHolder : IRoomObjectHolder
    {
        public string LogicType { get; }
    }
}
