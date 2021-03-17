using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Messages
{
    public class RoomObjectUpdateMessage
    {
        public IPoint Location { get; private set; }

        public RoomObjectUpdateMessage(IPoint location)
        {
            Location = location;
        }
    }
}
