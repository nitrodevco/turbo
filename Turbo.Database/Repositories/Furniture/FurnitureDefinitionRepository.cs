using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Furniture;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Database.Repositories.Furniture
{
    public class FurnitureDefinitionRepository : IFurnitureDefinitionRepository
    {
        private readonly IEmulatorContext _context;

        public FurnitureDefinitionRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<FurnitureDefinitionEntity> FindAsync(int id) => await _context.FurnitureDefinitions
            .FindAsync(id);

        public async Task<List<FurnitureDefinitionEntity>> FindAllAsync() => await _context.FurnitureDefinitions
            .ToListAsync();
    }
}
