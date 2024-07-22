using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Catalog;

public interface ICatalogManager : IComponent
{
    public IDictionary<string, ICatalog> Catalogs { get; }
    public ICatalogPage GetRootForPlayer(IPlayer player, string catalogType);

    public Task<bool> PurchaseOfferForPlayer(IPlayer player, string catalogType, int pageId, int offerId,
        string extraParam, int quantity);

    public ICatalogOffer GetOfferForPlayer(IPlayer player, string catalogType, int offerId);
    public ICatalogPage GetPageForPlayer(IPlayer player, string catalogType, int pageId);
}