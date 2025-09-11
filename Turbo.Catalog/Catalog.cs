using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Catalog.Factories;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;
using Turbo.Database.Repositories.Catalog;

namespace Turbo.Catalog;

public class Catalog(
    ILogger<ICatalog> _logger,
    IFurnitureManager _furnitureManager,
    ICatalogFactory _catalogFactory,
    IServiceScopeFactory _serviceScopeFactory,
    string _catalogType) : Component, ICatalog
{
    public string CatalogType => _catalogType;
    public ICatalogPage Root { get; private set; }
    public IDictionary<int, ICatalogPage> Pages { get; } = new Dictionary<int, ICatalogPage>();
    public IDictionary<int, ICatalogOffer> Offers { get; } = new Dictionary<int, ICatalogOffer>();
    public IDictionary<int, ICatalogProduct> Products { get; } = new Dictionary<int, ICatalogProduct>();

    protected override async Task OnInit()
    {
        await LoadCatalog();
    }

    private async Task LoadCatalog()
    {
        Pages.Clear();
        Offers.Clear();
        Products.Clear();

        using var scope = _serviceScopeFactory.CreateScope();

        var catalogPageRepository = scope.ServiceProvider.GetService<ICatalogPageRepository>();
        var catalogOfferRepository = scope.ServiceProvider.GetService<ICatalogOfferRepository>();
        var catalogProductRepository = scope.ServiceProvider.GetService<ICatalogProductRepository>();
        var catalogPageOfferRepository = scope.ServiceProvider.GetService<ICatalogPageOfferRepository>();

        var pageEntities = await catalogPageRepository.FindAllAsync();
        var offerEntities = await catalogOfferRepository.FindAllAsync();
        var productEntities = await catalogProductRepository.FindAllAsync();

        if (pageEntities is not null)
        {
            pageEntities = pageEntities.OrderBy(entity => entity.Localization).ToList();

            foreach (var pageEntity in pageEntities)
            {
                if (!pageEntity.Visible ?? false) continue;

                var catalogPage = _catalogFactory.CreatePage(pageEntity);

                Pages.Add(catalogPage.Id, catalogPage);
            }
        }

        // load page-offers per page and create offers bound to the page mapping
        if (Pages.Count > 0 && catalogPageOfferRepository is not null)
        {
            foreach (var page in Pages.Values)
            {
                if (page.Id == -1) continue; // skip root

                var pageOffers = await catalogPageOfferRepository.FindAllByPageIdAsync(page.Id);
                if (pageOffers is null) continue;

                // order by order_index ascending (nulls last)
                foreach (var pageOfferEntity in pageOffers)
                {
                    var offer = _catalogFactory.CreateOffer(pageOfferEntity);
                    Offers[offer.Id] = offer; // key by page-offer id
                }
            }
        }

        if (productEntities is not null)
            foreach (var productEntity in productEntities)
            {
                var product = _catalogFactory.CreateProduct(productEntity);

                Products.Add(product.Id, product);
            }

        BuildCatalog();
    }

    private void BuildCatalog()
    {
        Root = _catalogFactory.CreateRoot();

        Pages.Add(-1, Root);

        foreach (var offer in Offers.Values)
        {
            if (offer is null) continue;

            if (!Pages.ContainsKey(offer.PageId)) continue;

            offer.SetPage(Pages[offer.PageId]);
        }

        foreach (var product in Products.Values)
        {
            if (product is null) continue;

            // products map to base offer id; need to link to all page-offers that reference that base offer
            foreach (var offer in Offers.Values)
            {
                if (offer is null) continue;

                if (product.OfferId == offer.OfferEntityId)
                {
                    product.SetOffer(offer);
                }
            }
        }

        foreach (var page in Pages.Values)
        {
            if (page is null) continue;

            if (!Pages.ContainsKey(page.ParentId)) continue;

            page.SetParent(Pages[page.ParentId]);

            page.CacheOfferIds();
        }
    }

    public ICatalogPage GetRootForPlayer(IPlayer player)
    {
        if (player is null) return null;

        return Root;
    }

    public ICatalogPage GetPageForPlayer(IPlayer player, int pageId)
    {
        if (player is null) return null;

        if (Pages.TryGetValue(pageId, out var page)) return page;

        return null;
    }

    public ICatalogOffer GetOfferForPlayer(IPlayer player, int offerId)
    {
        if (player is null) return null;

        if (Offers.TryGetValue(offerId, out var offer))
        {
            Console.WriteLine($"Offer Id: {offer.Id}, PageId: {offer.PageId}, LocalizationId: {offer.LocalizationId}, CostCredits: {offer.CostCredits}, CostCurrency: {offer.CostCurrency}, CurrencyType: {offer.CurrencyType}, ClubLevel: {offer.ClubLevel}");
            return offer;
        }

        return null;
    }

    public async Task<ICatalogOffer> PurchaseOffer(IPlayer player, int pageId, int offerId, string extraParam,
        int quantity)
    {
        var page = GetPageForPlayer(player, pageId);

        if (page is null) return null;

        return await page.PurchaseOffer(player, offerId, extraParam, quantity);
    }

    protected override async Task OnDispose()
    {
        try
        {
            Pages.Clear();
            Offers.Clear();
            Products.Clear();
        }
        catch (Exception)
        {
        }
    }
}