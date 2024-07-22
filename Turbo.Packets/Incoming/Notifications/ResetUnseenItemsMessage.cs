using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Notifications;

public class ResetUnseenItemsMessage : IMessageEvent
{
    public int CategoryId { get; init; }
    public int[] ItemIds { get; init; }
}