using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Catalog;

public class CatalogPage(
    ILogger<ICatalogPage> _logger,
    CatalogPageEntity _entity) : ICatalogPage
{
    public ICatalogPage Parent { get; private set; }
    public IDictionary<int, ICatalogPage> Children { get; } = new Dictionary<int, ICatalogPage>();
    public IDictionary<int, ICatalogOffer> Offers { get; } = new Dictionary<int, ICatalogOffer>();
    public IList<int> OfferIds { get; private set; } = [];
    public IList<string> ImageDatas { get; } = [];
    public IList<string> TextDatas { get; } = [];

    public virtual void SetParent(ICatalogPage catalogPage)
    {
        if (catalogPage == null || catalogPage == Parent) return;

        Parent = catalogPage;

        Parent.AddChild(this);
    }

    public virtual void AddChild(ICatalogPage catalogPage)
    {
        if (catalogPage == null || Children.ContainsKey(catalogPage.Id)) return;

        Children.Add(catalogPage.Id, catalogPage);

        catalogPage.SetParent(this);
    }

    public virtual void AddOffer(ICatalogOffer catalogOffer)
    {
        if (catalogOffer == null || Offers.ContainsKey(catalogOffer.Id)) return;

        Offers.Add(catalogOffer.Id, catalogOffer);

        catalogOffer.SetPage(this);
    }

    public virtual void CacheOfferIds()
    {
        OfferIds = [.. Offers.Keys];
    }

    public async Task<ICatalogOffer> PurchaseOffer(IPlayer player, int offerId, string extraParam, int quantity)
    {
        if (player == null) return null;

        if (Offers.TryGetValue(offerId, out var offer)) return await offer.Purchase(player, extraParam, quantity);

        return null;
    }

    public virtual int Id => _entity.Id;
    public virtual int ParentId => _entity.ParentEntityId ?? -1;
    public virtual int Icon => _entity.Icon;
    public virtual string Name => _entity.Name;
    public virtual string Localization => _entity.Localization;
    public virtual string Layout => _entity.Layout;
    public virtual bool Visible => _entity.Visible ?? false;
}