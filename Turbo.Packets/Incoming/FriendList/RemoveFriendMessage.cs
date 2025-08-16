using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.FriendList;

public record RemoveFriendMessage : IMessageEvent
{
    public required List<int> FriendIds { get; init; }
}