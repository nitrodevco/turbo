using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Packets.Incoming.Navigator
{
    public record CanCreateRoomMessage : IMessageEvent
    {
        public int PlayerCurrentRoomsCount { get; init; }
        public int PlayerMaxRoomsCount { get; init; }
    }
}
