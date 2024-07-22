using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record PurchaseVipMembershipExtensionMessage : IMessageEvent
{
    public int OfferId { get; init; }
}