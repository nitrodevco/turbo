using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Catalog.Constants;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Catalog;

public class CatalogOffer(
    ILogger<ICatalogOffer> _logger,
    CatalogPageOfferEntity _entity) : ICatalogOffer
{
    public int Id => _entity.Id; // page-offer id
    public int PageId => _entity.CatalogPageEntityId;
    public int OfferEntityId => _entity.CatalogOfferEntityId; // base offer id
    public string LocalizationId => _entity.Offer.LocalizationId;
    public int CostCredits => _entity.CostCredits;
    public int CostCurrency => _entity.CostCurrency;
    public int? CurrencyType => _entity.CurrencyType;
    public int CostSilver => _entity.CostSilver;
    public int OrderIndex => _entity.OrderIndex ?? int.MaxValue;
    public bool CanGift => _entity.Offer.CanGift ?? false;
    public bool CanBundle => _entity.Offer.CanBundle ?? false;
    public int ClubLevel => _entity.Offer.ClubLevel;
    public bool IsPet => Products.Count >= 1 ? Products[0].ProductType.Equals(ProductTypeEnum.Pet) : false;
    public string PreviewImage => string.Empty;
    public bool Visible => _entity.Visible ?? false;
    public ICatalogPage Page { get; private set; }
    public IList<ICatalogProduct> Products { get; } = [];

    public void SetPage(ICatalogPage catalogPage)
    {
        if (catalogPage is null || Page == catalogPage) return;

        Page = catalogPage;

        catalogPage.AddOffer(this);
    }

    public void AddProduct(ICatalogProduct catalogProduct)
    {
        if (catalogProduct is null || Products.Contains(catalogProduct)) return;

        Products.Add(catalogProduct);

        catalogProduct.SetOffer(this);
    }

    public async Task<ICatalogOffer> Purchase(IPlayer player, string extraParam, int quantity)
    {
        if (player is null || quantity <= 0 || !Visible || Products.Count == 0) return null;

        foreach (var product in Products)
            if (!product.CanPlayerRecieveProduct(player))
                return null;

        var totalCreditsCost = CostCredits * quantity;
        var totalCurrencyCost = CostCurrency * quantity;

        // TODO: check club level and deduct costs from wallet

        for (var i = 0; i < quantity; i++)
            foreach (var product in Products)
                await product.GiveProductToPlayer(player);

        return this;
    }
}