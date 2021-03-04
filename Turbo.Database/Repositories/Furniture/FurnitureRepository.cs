using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Entities.Players;
using Turbo.Database.Repositories.Player;

namespace Turbo.Database.Repositories.Furniture
{
    public class FurnitureRepository : IFurnitureRepository
    {
        private readonly IEmulatorContext _context;

        public FurnitureRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<FurnitureEntity> FindAsync(int id) => await _context.Furnitures.FindAsync(id);
    }
}
