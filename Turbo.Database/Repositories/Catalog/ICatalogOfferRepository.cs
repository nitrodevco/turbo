using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Database.Repositories.Catalog
{
    public interface ICatalogOfferRepository : IBaseRepository<CatalogOfferEntity>
    {
        public Task<List<CatalogOfferEntity>> FindAllAsync();
    }
}