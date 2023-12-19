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
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Catalog;
using Turbo.Database.Repositories.Catalog;

namespace Turbo.Catalog
{
    public class Catalog : Component, ICatalog
    {
        private readonly ILogger<ICatalog> _logger;
        private readonly IFurnitureManager _furnitureManager;
        private readonly ICatalogFactory _catalogFactory;
        private readonly ICatalogPageRepository _pageRepository;
        private readonly ICatalogOfferRepository _offerRepository;
        private readonly ICatalogProductRepository _productRepository;
        private readonly string _catalogType;

        public ICatalogPage Root { get; private set; }
        public IDictionary<int, ICatalogPage> Pages { get; private set; } = new Dictionary<int, ICatalogPage>();
        public IDictionary<int, ICatalogOffer> Offers { get; private set; } = new Dictionary<int, ICatalogOffer>();
        public IDictionary<int, ICatalogProduct> Products { get; private set; } = new Dictionary<int, ICatalogProduct>();

        public Catalog(
            ILogger<ICatalog> logger,
            IFurnitureManager furnitureManager,
            ICatalogFactory catalogFactory,
            ICatalogPageRepository catalogPageRepository,
            ICatalogOfferRepository catalogOfferRepository,
            ICatalogProductRepository catalogProductRepository,
            string catalogType)
        {
            _logger = logger;
            _furnitureManager = furnitureManager;
            _catalogFactory = catalogFactory;
            _pageRepository = catalogPageRepository;
            _offerRepository = catalogOfferRepository;
            _productRepository = catalogProductRepository;
            _catalogType = catalogType;
        }

        protected override async Task OnInit()
        {
            await LoadCatalog();
        }

        protected override async Task OnDispose()
        {

        }

        public ICatalogPage GetRootForPlayer(IPlayer player)
        {
            if (player == null) return null;

            return Root;
        }

        public ICatalogPage GetPageForPlayer(IPlayer player, int pageId)
        {
            if(player == null) return null;

            if(Pages.TryGetValue(pageId, out var page)) return page;

            return null;
        }

        public ICatalogOffer GetOfferForPlayer(IPlayer player, int offerId)
        {
            if(player == null) return null;

            if(Offers.TryGetValue(offerId, out var offer)) return offer;

            return null;
        }

        public async Task<ICatalogOffer> PurchaseOffer(IPlayer player, int pageId, int offerId, string extraParam, int quantity)
        {
            var page = GetPageForPlayer(player, pageId);

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

            var pageEntities = await _pageRepository.FindAllAsync();
            var offerEntities = await _offerRepository.FindAllAsync();
            var productEntities = await _productRepository.FindAllAsync();

            if (pageEntities != null)
            {
                pageEntities = pageEntities.OrderBy(entity => entity.Localization).ToList();

                foreach (var pageEntity in pageEntities)
                {
                    if (!pageEntity.Visible ?? false) continue;

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
