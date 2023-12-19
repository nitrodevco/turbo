using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Database.Repositories.Catalog
{
    public class CatalogPageRepository : ICatalogPageRepository
    {
        private readonly IEmulatorContext _context;

        public CatalogPageRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<CatalogPageEntity> FindAsync(int id) => await _context.CatalogPages
            .FirstOrDefaultAsync(page => page.Id == id);

        public async Task<List<CatalogPageEntity>> FindAllAsync() => await _context.CatalogPages
            .AsNoTracking()
            .ToListAsync();
    }
}