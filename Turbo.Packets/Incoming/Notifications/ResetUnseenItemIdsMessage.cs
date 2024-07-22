using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Notifications;

public class ResetUnseenItemIdsMessage : IMessageEvent
{
    public int CategoryId { get; init; }
}