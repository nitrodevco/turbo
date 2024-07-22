using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record SetTargetedOfferStateMessage : IMessageEvent
{
    public int TargetedOfferId { get; init; }
    public int TrackingState { get; init; }
}