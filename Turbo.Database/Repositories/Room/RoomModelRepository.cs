using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room
{
    public class RoomModelRepository(IEmulatorContext _context) : IRoomModelRepository
    {
        public async Task<RoomModelEntity> FindAsync(int id) => await _context.RoomModels
            .FirstOrDefaultAsync(roomModel => roomModel.Id == id);

        public async Task<RoomModelEntity> FindByNameAsync(string name) => await _context.RoomModels
            .FirstOrDefaultAsync(roomModel => roomModel.Name == name);

        public async Task<List<RoomModelEntity>> FindAllAsync() => await _context.RoomModels
            .AsNoTracking()
            .ToListAsync();
    }
}
