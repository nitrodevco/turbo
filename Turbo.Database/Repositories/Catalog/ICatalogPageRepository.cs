using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Database.Repositories.Catalog;

public interface ICatalogPageRepository : IBaseRepository<CatalogPageEntity>
{
    public Task<List<CatalogPageEntity>> FindAllAsync();
}