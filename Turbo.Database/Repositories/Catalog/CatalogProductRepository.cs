using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Database.Repositories.Catalog
{
    public class CatalogProductRepository : ICatalogProductRepository
    {
        private readonly IEmulatorContext _context;

        public CatalogProductRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<CatalogProductEntity> FindAsync(int id) => await _context.CatalogProducts
            .FirstOrDefaultAsync(page => page.Id == id);

        public async Task<List<CatalogProductEntity>> FindAllAsync() => await _context.CatalogProducts
            .AsNoTracking()
            .ToListAsync();
    }
}