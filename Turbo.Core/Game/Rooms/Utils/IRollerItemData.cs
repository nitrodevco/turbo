using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Rooms.Utils
{
    public interface IRollerItemData
    {
        public IRoomObject RoomObject { get; set; }
        public double Height { get; set; }
        public double HeightNext { get; set; }
    }
}
