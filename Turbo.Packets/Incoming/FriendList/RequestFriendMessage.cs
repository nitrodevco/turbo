using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.FriendList;
public record RequestFriendMessage : IMessageEvent
{
    public required string PlayerName { get; init; }
}
