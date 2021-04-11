using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Object.Logic.Wired
{
    public interface IWiredArguments
    {
        public IRoomObject? UserObject { get; set; }
        public IRoomObject? FurnitureObject { get; set; }
    }
}
