using Turbo.Core.Game.Catalog;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Catalog;

public record ProductOfferMessage : IComposer
{
    public ICatalogOffer Offer { get; init; }
}