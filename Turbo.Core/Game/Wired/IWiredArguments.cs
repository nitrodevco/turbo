using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Wired
{
    public interface IWiredArguments
    {
        public IRoomObjectAvatar? UserObject { get; set; }
        public IRoomObjectFloor? FurnitureObject { get; set; }
    }
}
