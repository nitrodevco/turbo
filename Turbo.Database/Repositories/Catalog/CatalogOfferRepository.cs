using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Database.Repositories.Catalog
{
    public class CatalogOfferRepository : ICatalogOfferRepository
    {
        private readonly IEmulatorContext _context;

        public CatalogOfferRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<CatalogOfferEntity> FindAsync(int id) => await _context.CatalogOffers
            .FirstOrDefaultAsync(page => page.Id == id);

        public async Task<IList<CatalogOfferEntity>> FindAllAsync() => await _context.CatalogOffers
            .ToListAsync();
    }
}