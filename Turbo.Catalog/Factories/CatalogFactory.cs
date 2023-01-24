using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Catalog.Constants;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Storage;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Catalog.Factories
{
    public class CatalogFactory : ICatalogFactory
    {
        private readonly IServiceProvider _provider;

        public CatalogFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ICatalog CreateCatalog(string catalogType)
        {
            var logger = _provider.GetService<ILogger<ICatalog>>();
            var furnitureManager = _provider.GetService<IFurnitureManager>();
            var scopeFactory = _provider.GetService<IServiceScopeFactory>();

            if (logger == null || furnitureManager == null || scopeFactory == null) return null;

            return new Catalog(logger, furnitureManager, this, scopeFactory, catalogType);
        }

        public ICatalogPage CreateRoot()
        {
            var logger = _provider.GetService<ILogger<ICatalogPage>>();

            if (logger == null) return null;

            return new CatalogRoot(logger);
        }

        public ICatalogPage CreatePage(CatalogPageEntity entity)
        {
            var logger = _provider.GetService<ILogger<ICatalogPage>>();

            if (logger == null) return null;

            return new CatalogPage(logger, entity);
        }

        public ICatalogOffer CreateOffer(CatalogOfferEntity entity)
        {
            var logger = _provider.GetService<ILogger<ICatalogOffer>>();

            if (logger == null) return null;

            return new CatalogOffer(logger, entity);
        }

        public ICatalogProduct CreateProduct(CatalogProductEntity entity)
        {
            var logger = _provider.GetService<ILogger<ICatalogProduct>>();

            if (logger == null) return null;

            var product = new CatalogProduct(logger, entity);

            if (product.ProductType.Equals(ProductType.Floor) || product.ProductType.Equals(ProductType.Wall))
            {
                var furnitureManager = _provider.GetService<IFurnitureManager>();

                var definition = furnitureManager.GetFurnitureDefinition(product.FurnitureDefinitionId);

                if (definition != null) product.SetFurnitureDefinition(definition);
            }

            return product;
        }
    }
}