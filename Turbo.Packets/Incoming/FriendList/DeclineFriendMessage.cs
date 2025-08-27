using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.FriendList;
public record DeclineFriendMessage : IMessageEvent
{
    public bool DeclineAll { get; init; }
    public List<int>? Friends { get; init; }
}
