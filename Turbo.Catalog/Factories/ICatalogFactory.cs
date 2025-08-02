using Turbo.Core.Database.Entities.Catalog;
using Turbo.Core.Game.Catalog;

namespace Turbo.Core.Database.Factories.Catalog;

public interface ICatalogFactory
{
    public ICatalog CreateCatalog(string catalogType);
    public ICatalogPage CreateRoot();
    public ICatalogPage CreatePage(CatalogPageEntity entity);
    public ICatalogOffer CreateOffer(CatalogOfferEntity entity);
    public ICatalogProduct CreateProduct(CatalogProductEntity entity);
}