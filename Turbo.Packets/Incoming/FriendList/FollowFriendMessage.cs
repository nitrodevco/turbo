using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.FriendList;

public record FollowFriendMessage : IMessageEvent
{
    public required int PlayerId { get; init; }
}