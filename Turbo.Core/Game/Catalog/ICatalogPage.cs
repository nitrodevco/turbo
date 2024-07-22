using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Game.Catalog;

public interface ICatalogPage
{
    public ICatalogPage Parent { get; }
    public IDictionary<int, ICatalogPage> Children { get; }
    public IDictionary<int, ICatalogOffer> Offers { get; }
    public IList<int> OfferIds { get; }
    public IList<string> ImageDatas { get; }
    public IList<string> TextDatas { get; }

    public int Id { get; }
    public int ParentId { get; }
    public int Icon { get; }
    public string Name { get; }
    public string Localization { get; }
    public string Layout { get; }
    public bool Visible { get; }

    public void SetParent(ICatalogPage catalogPage);
    public void AddChild(ICatalogPage catalogPage);
    public void AddOffer(ICatalogOffer catalogItem);
    public void CacheOfferIds();
    public Task<ICatalogOffer> PurchaseOffer(IPlayer player, int offerId, string extraParam, int quantity);
}