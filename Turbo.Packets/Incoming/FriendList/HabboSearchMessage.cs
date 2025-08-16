using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.FriendList;

public record HabboSearchMessage : IMessageEvent
{
    public required string SearchQuery { get; init; }
}
