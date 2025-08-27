using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.FriendList;

public record GetMessengerHistoryMessage : IMessageEvent
{
    public int ChatId { get; init; }
    public string Message { get; init; }
}