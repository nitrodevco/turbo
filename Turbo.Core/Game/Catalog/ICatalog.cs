using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Catalog;

public interface ICatalog : IComponent
{
    public IDictionary<int, ICatalogPage> Pages { get; }
    public IDictionary<int, ICatalogOffer> Offers { get; }
    public IDictionary<int, ICatalogProduct> Products { get; }

    public string CatalogType { get; }

    public ICatalogPage GetRootForPlayer(IPlayer player);
    public ICatalogPage GetPageForPlayer(IPlayer player, int pageId);
    public ICatalogOffer GetOfferForPlayer(IPlayer player, int offerId);
    public Task<ICatalogOffer> PurchaseOffer(IPlayer player, int pageId, int offerId, string extraParam, int quantity);
}