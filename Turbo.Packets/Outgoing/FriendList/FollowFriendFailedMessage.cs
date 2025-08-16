using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;

public record FollowFriendFailedMessage : IComposer
{
    public int ErrorCode { get; init; }
}
