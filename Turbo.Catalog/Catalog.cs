using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Catalog.Factories;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Catalog.Constants;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Catalog;
using Turbo.Database.Repositories.Catalog;

namespace Turbo.Catalog
{
    public class Catalog : ICatalog
    {
        private readonly ILogger<ICatalog> _logger;
        private readonly IFurnitureManager _furnitureManager;
        private readonly ICatalogFactory _catalogFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _catalogType;

        public ICatalogPage Root { get; private set; }
        public IDictionary<int, ICatalogPage> Pages { get; private set; }
        public IDictionary<int, ICatalogOffer> Offers { get; private set; }
        public IDictionary<int, ICatalogProduct> Products { get; private set; }

        public Catalog(
            ILogger<ICatalog> logger,
            IFurnitureManager furnitureManager,
            ICatalogFactory catalogFactory,
            IServiceScopeFactory scopeFactory,
            string catalogType)
        {
            _logger = logger;
            _furnitureManager = furnitureManager;
            _catalogFactory = catalogFactory;
            _serviceScopeFactory = scopeFactory;
            _catalogType = catalogType;

            Pages = new Dictionary<int, ICatalogPage>();
            Offers = new Dictionary<int, ICatalogOffer>();
            Products = new Dictionary<int, ICatalogProduct>();
        }

        public async ValueTask InitAsync()
        {
            await LoadCatalog();
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }

        public ICatalogPage GetRootForPlayer(IPlayer player)
        {
            if (player == null) return null;

            return Root;
        }

        public ICatalogPage GetPageForPlayer(IPlayer player, int pageId, int offerId)
        {
            if (player == null || !Pages.ContainsKey(pageId)) return null;

            return Pages[pageId];
        }

        public async Task<ICatalogOffer> PurchaseOffer(IPlayer player, int pageId, int offerId, string extraParam, int quantity)
        {
            var page = GetPageForPlayer(player, pageId, offerId);

            if (page == null) return null;

            return await page.PurchaseOffer(player, offerId, extraParam, quantity);
        }

        private void BuildCatalog()
        {
            Root = _catalogFactory.CreateRoot();

            Pages.Add(-1, Root);

            foreach (var offer in Offers.Values)
            {
                if (offer == null) continue;

                if (!Pages.ContainsKey(offer.PageId)) continue;

                offer.SetPage(Pages[offer.PageId]);
            }

            foreach (var product in Products.Values)
            {
                if (product == null) continue;

                if (!Offers.ContainsKey(product.OfferId)) continue;

                product.SetOffer(Offers[product.OfferId]);
            }

            foreach (var page in Pages.Values)
            {
                if (page == null) continue;

                if (!Pages.ContainsKey(page.ParentId)) continue;

                page.SetParent(Pages[page.ParentId]);

                page.CacheOfferIds();
            }
        }

        private async Task LoadCatalog()
        {
            Pages.Clear();
            Offers.Clear();
            Products.Clear();

            IList<CatalogPageEntity> pageEntities = null;
            IList<CatalogOfferEntity> offerEntities = null;
            IList<CatalogProductEntity> productEntities = null;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                pageEntities = await scope.ServiceProvider.GetService<ICatalogPageRepository>()?.FindAllAsync();
                offerEntities = await scope.ServiceProvider.GetService<ICatalogOfferRepository>()?.FindAllAsync();
                productEntities = await scope.ServiceProvider.GetService<ICatalogProductRepository>()?.FindAllAsync();
            }

            if (pageEntities != null)
            {
                foreach (var pageEntity in pageEntities)
                {
                    if (!pageEntity.Visible) continue;

                    var catalogPage = _catalogFactory.CreatePage(pageEntity);

                    Pages.Add(catalogPage.Id, catalogPage);
                }
            }

            if (offerEntities != null)
            {
                foreach (var offerEntity in offerEntities)
                {
                    var offer = _catalogFactory.CreateOffer(offerEntity);

                    Offers.Add(offer.Id, offer);
                }
            }

            if (productEntities != null)
            {
                foreach (var productEntity in productEntities)
                {
                    var product = _catalogFactory.CreateProduct(productEntity);

                    Products.Add(product.Id, product);
                }
            }

            BuildCatalog();
        }

        public string CatalogType => _catalogType;
    }
}
