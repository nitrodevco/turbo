using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record PurchaseBasicMembershipExtensionMessage : IMessageEvent
{
    public int OfferId { get; init; }
}