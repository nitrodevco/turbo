using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Database.Repositories.Catalog;
public interface ICatalogPageOfferRepository : IBaseRepository<CatalogPageOfferEntity>
{
    Task<List<CatalogPageOfferEntity>> FindAllByPageIdAsync(int pageId);
}
