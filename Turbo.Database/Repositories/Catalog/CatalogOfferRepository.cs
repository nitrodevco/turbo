using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Core.Database.Entities.Catalog;

namespace Turbo.Database.Repositories.Catalog;

public class CatalogOfferRepository(IEmulatorContext _context) : ICatalogOfferRepository
{
    public async Task<CatalogOfferEntity> FindAsync(int id)
    {
        return await _context.CatalogOffers
            .FirstOrDefaultAsync(page => page.Id == id);
    }

    public async Task<List<CatalogOfferEntity>> FindAllAsync()
    {
        return await _context.CatalogOffers
            .AsNoTracking()
            .ToListAsync();
    }
}