using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Inventory.Badges;

public record SetActivatedBadgesMessage : IMessageEvent
{
    public IDictionary<int, string> Badges { get; init; }
}