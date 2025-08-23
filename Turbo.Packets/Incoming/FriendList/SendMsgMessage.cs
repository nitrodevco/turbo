using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.FriendList;

public record SendMsgMessage : IMessageEvent
{
    public int ChatId { get; init; }
    public string MessageText { get; init; }
    public int ConfirmationId { get; init; }
}
