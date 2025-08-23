using Turbo.Core.Game.Messenger.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.FriendList;

public record FollowFriendFailedMessage : IComposer
{
    public FollowFriendErrorEnum ErrorCode { get; init; }
}
