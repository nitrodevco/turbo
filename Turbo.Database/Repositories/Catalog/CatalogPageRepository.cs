using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Entities.Catalog;
using Turbo.Database.Context;

namespace Turbo.Database.Repositories.Catalog;

public class CatalogPageRepository(IEmulatorContext _context) : ICatalogPageRepository
{
    public async Task<CatalogPageEntity> FindAsync(int id)
    {
        return await _context.CatalogPages
            .FirstOrDefaultAsync(page => page.Id == id);
    }

    public async Task<List<CatalogPageEntity>> FindAllAsync()
    {
        return await _context.CatalogPages
            .AsNoTracking()
            .ToListAsync();
    }
}