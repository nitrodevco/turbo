using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record GetNextTargetedOfferMessage : IMessageEvent
{
    public int OfferId { get; init; }
}