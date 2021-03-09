using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Action
{
    public record RemoveRightsMessage : IMessageEvent
    {
        public List<int> UserIds { get; init; }
    }
}
