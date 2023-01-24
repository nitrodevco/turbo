using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Catalog
{
    public class CatalogProduct : ICatalogProduct
    {
        private readonly ILogger<ICatalogProduct> _logger;
        private readonly CatalogProductEntity _entity;

        public ICatalogOffer Offer { get; private set; }
        public IFurnitureDefinition FurnitureDefinition { get; private set; }

        public CatalogProduct(
            ILogger<ICatalogProduct> logger,
            CatalogProductEntity entity
        )
        {
            _logger = logger;
            _entity = entity;
        }

        public void SetOffer(ICatalogOffer catalogOffer)
        {
            if ((catalogOffer == null) || Offer == catalogOffer) return;

            Offer = catalogOffer;

            catalogOffer.AddProduct(this);
        }

        public void SetFurnitureDefinition(IFurnitureDefinition furnitureDefinition)
        {
            if ((furnitureDefinition == null) || (furnitureDefinition.Id != _entity.FurnitureDefinitionEntityId)) return;

            FurnitureDefinition = furnitureDefinition;
        }

        public int Id => _entity.Id;
        public int OfferId => _entity.CatalogOfferEntityId;
        public int FurnitureDefinitionId => _entity.FurnitureDefinitionEntityId ?? -1;
        public string ProductType => _entity.ProductType;
        public int SpriteId => FurnitureDefinition?.SpriteId ?? -1;
        public string ExtraParam => _entity.ExtraParam;
        public int Quantity => _entity.Quantity;
        public int UniqueSize => _entity.UniqueSize;
        public int UniqueRemaining => _entity.UniqueRemaining;
    }
}