using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Object.Logic.Wired
{
    public interface IWiredArguments
    {
        public IRoomObjectAvatar? UserObject { get; set; }
        public IRoomObjectFloor? FurnitureObject { get; set; }
    }
}
