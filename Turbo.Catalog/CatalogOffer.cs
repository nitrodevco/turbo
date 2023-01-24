using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Catalog.Constants;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Catalog
{
    public class CatalogOffer : ICatalogOffer
    {
        private readonly ILogger<ICatalogOffer> _logger;
        private readonly CatalogOfferEntity _entity;

        public ICatalogPage Page { get; private set; }
        public IList<ICatalogProduct> Products { get; private set; }

        public CatalogOffer(
            ILogger<ICatalogOffer> logger,
            CatalogOfferEntity entity)
        {
            _logger = logger;
            _entity = entity;

            Products = new List<ICatalogProduct>();
        }

        public void SetPage(ICatalogPage catalogPage)
        {
            if ((catalogPage == null) || Page == catalogPage) return;

            Page = catalogPage;

            catalogPage.AddOffer(this);
        }

        public void AddProduct(ICatalogProduct catalogProduct)
        {
            if ((catalogProduct == null) || Products.Contains(catalogProduct)) return;

            Products.Add(catalogProduct);

            catalogProduct.SetOffer(this);
        }

        public int Id => _entity.Id;
        public int PageId => _entity.CatalogPageEntityId;
        public string LocalizationId => _entity.LocalizationId;
        public int CostCredits => _entity.CostCredits;
        public int CostCurrency => _entity.CostCurrency;
        public int? CurrencyType => _entity.CurrencyType;
        public bool CanGift => _entity.CanGift;
        public bool CanBundle => _entity.CanBundle;
        public int ClubLevel => _entity.ClubLevel;
        public bool IsPet => (Products.Count >= 1) ? Products[0].ProductType.Equals(ProductType.Pet) : false;
        public string PreviewImage => "";
        public bool Visible => _entity.Visible;
    }
}