using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.FriendList;

public record SendRoomInviteMessage : IMessageEvent
{
    public required string Message { get; init; }
    public required List<int> FriendIds { get; init; }
}
