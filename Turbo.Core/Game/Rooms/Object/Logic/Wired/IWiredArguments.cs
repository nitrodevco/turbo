using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Object.Logic.Wired
{
    public interface IWiredArguments
    {
        public IRoomObject? RoomObject { get; set; }
    }
}
