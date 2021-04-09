using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Object.Logic.Furniture.Wired.Arguments
{
    public class WiredWalksOnFurniArguments : WiredArguments
    {
        public IRoomObject FurnitureObject { get; set; }
    }
}
