using System.Collections.Generic;

namespace Turbo.Packets.Incoming.Room.Action
{
    public record RemoveRightsMessage : IMessageEvent
    {
        public List<int> UserIds { get; init; }
    }
}
