using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.FriendList;

public record VisitUserMessage : IMessageEvent
{
    public string PlayerName { get; init; }
}
