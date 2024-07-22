using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record ShopTargetedOfferViewedMessage : IMessageEvent
{
    public int TargetedOfferId { get; init; }
    public int TrackingState { get; init; }
}