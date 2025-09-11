using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Game.Catalog;

public interface ICatalogPage
{
    int Id { get; }
    int ParentId { get; }
    int Icon { get; }
    string Name { get; }
    string Localization { get; }
    string Layout { get; }
    bool Visible { get; }
    ICatalogPage Parent { get; }
    IDictionary<int, ICatalogPage> Children { get; }
    IDictionary<int, ICatalogOffer> Offers { get; }
    IList<int> OfferIds { get; }
    IList<string> ImageDatas { get; }
    IList<string> TextDatas { get; }
    void SetParent(ICatalogPage catalogPage);
    void AddChild(ICatalogPage catalogPage);
    void AddOffer(ICatalogOffer catalogItem);
    void CacheOfferIds();
    Task<ICatalogOffer> PurchaseOffer(IPlayer player, int offerId, string extraParam, int quantity);
}