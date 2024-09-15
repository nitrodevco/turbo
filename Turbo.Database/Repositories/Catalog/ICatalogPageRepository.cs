using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Catalog;

namespace Turbo.Database.Repositories.Catalog;

public interface ICatalogPageRepository : IBaseRepository<CatalogPageEntity>
{
    public Task<List<CatalogPageEntity>> FindAllAsync();
}