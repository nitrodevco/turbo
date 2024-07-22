using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record PurchaseTargetedOfferMessage : IMessageEvent
{
    public int OfferId { get; init; }
    public int Quantity { get; init; }
}