using Turbo.Core.Game.Catalog;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Catalog;

public record PurchaseOkMessage : IComposer
{
    public ICatalogOffer Offer { get; init; }
}