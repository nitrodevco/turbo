using System.Collections.Generic;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Catalog;

public record CatalogPageMessage : IComposer
{
    public int PageId { get; init; }
    public string CatalogType { get; init; }
    public string LayoutCode { get; init; }
    public IList<string> ImageDatas { get; init; }
    public IList<string> TextDatas { get; init; }
    public IList<ICatalogOffer> Offers { get; init; }
    public int OfferId { get; init; }
    public bool AcceptSeasonCurrencyAsCredits { get; init; }
    public IList<ICatalogFrontPageItem> FrontPageItems { get; init; }
}