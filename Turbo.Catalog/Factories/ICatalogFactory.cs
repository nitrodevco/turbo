using Turbo.Core.Game.Catalog;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Catalog.Factories;

public interface ICatalogFactory
{
    public ICatalog CreateCatalog(string catalogType);
    public ICatalogPage CreateRoot();
    public ICatalogPage CreatePage(CatalogPageEntity entity);
    public ICatalogOffer CreateOffer(CatalogOfferEntity entity);
    public ICatalogProduct CreateProduct(CatalogProductEntity entity);
}