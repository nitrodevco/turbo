using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Database.Repositories.Furniture
{
    public class FurnitureRepository : IFurnitureRepository
    {
        private readonly IEmulatorContext _context;

        public FurnitureRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<FurnitureEntity> FindAsync(int id) => await _context.Furnitures
            .FirstOrDefaultAsync(furniture => furniture.Id == id);

        public async Task<List<FurnitureEntity>> FindAllByRoomIdAsync(int roomId) => await _context.Furnitures
            .Where(furniture => furniture.RoomEntityId == roomId)
            .ToListAsync();
    }
}
