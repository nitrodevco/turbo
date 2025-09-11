using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Catalog;

public interface ICatalog : IComponent
{
    string CatalogType { get; }
    IDictionary<int, ICatalogPage> Pages { get; }
    IDictionary<int, ICatalogOffer> Offers { get; }
    IDictionary<int, ICatalogProduct> Products { get; }
    ICatalogPage GetRootForPlayer(IPlayer player);
    ICatalogPage GetPageForPlayer(IPlayer player, int pageId);
    ICatalogOffer GetOfferForPlayer(IPlayer player, int offerId);
    Task<ICatalogOffer> PurchaseOffer(IPlayer player, int pageId, int offerId, string extraParam, int quantity);
}