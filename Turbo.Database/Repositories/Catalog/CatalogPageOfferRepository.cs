using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Database.Repositories.Catalog;
public class CatalogPageOfferRepository(IEmulatorContext context) : ICatalogPageOfferRepository
{
    public async Task<CatalogPageOfferEntity> FindAsync(int id)
    {
        return await context.CatalogPageOffers
            .Include(x => x.Offer)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<CatalogPageOfferEntity>> FindAllByPageIdAsync(int pageId)
    {
        return await context.CatalogPageOffers
            .Include(x => x.Offer)
            .Where(x => x.CatalogPageEntityId == pageId)
            .OrderBy(x => x.OrderIndex ?? int.MaxValue)
            .AsNoTracking()
            .ToListAsync();
    }
}
