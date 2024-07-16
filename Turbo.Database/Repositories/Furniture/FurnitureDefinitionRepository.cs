using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Database.Repositories.Furniture
{
    public class FurnitureDefinitionRepository(IEmulatorContext _context) : IFurnitureDefinitionRepository
    {
        public async Task<FurnitureDefinitionEntity> FindAsync(int id) => await _context.FurnitureDefinitions
            .FirstOrDefaultAsync(definition => definition.Id == id);

        public async Task<List<FurnitureDefinitionEntity>> FindAllAsync() => await _context.FurnitureDefinitions
            .AsNoTracking()
            .ToListAsync();
    }
}
