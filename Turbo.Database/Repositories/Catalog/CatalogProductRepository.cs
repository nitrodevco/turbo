using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Database.Repositories.Catalog
{
    public class CatalogProductRepository(IEmulatorContext _context) : ICatalogProductRepository
    {
        public async Task<CatalogProductEntity> FindAsync(int id) => await _context.CatalogProducts
            .FirstOrDefaultAsync(page => page.Id == id);

        public async Task<List<CatalogProductEntity>> FindAllAsync() => await _context.CatalogProducts
            .AsNoTracking()
            .ToListAsync();
    }
}