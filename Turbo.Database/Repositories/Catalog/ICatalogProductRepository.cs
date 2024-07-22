using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Database.Repositories.Catalog;

public interface ICatalogProductRepository : IBaseRepository<CatalogProductEntity>
{
    public Task<List<CatalogProductEntity>> FindAllAsync();
}