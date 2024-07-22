using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record GetProductOfferMessage : IMessageEvent
{
    public int OfferId { get; init; }
}