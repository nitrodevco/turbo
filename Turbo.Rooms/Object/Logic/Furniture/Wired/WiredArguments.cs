using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired
{
    public class WiredArguments : IWiredArguments
    {
        public IRoomObject RoomObject { get; set; }
    }
}
