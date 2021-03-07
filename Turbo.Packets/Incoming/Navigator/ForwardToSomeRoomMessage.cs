using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Packets.Incoming.Navigator
{
    public record ForwardToSomeRoomMessage : IMessageEvent
    {
        public RoomForwardType ForwardType { get; init; }
    }

    public enum RoomForwardType
    {
        RANDOM_FRIENDING_ROOM,
        PREDEFINED_NOOB_LOBBY,
        PREDIFINED_GROUP_LOBBY,
        PLAYER_HOME_ROOM
    }
}
