using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.FriendList;
public record AcceptFriendMessage : IMessageEvent
{
    public required List<int> Friends { get; init; }
}
