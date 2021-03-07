using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room
{
    public class RoomModelRepository : IRoomModelRepository
    {
        private readonly IEmulatorContext _context;

        public RoomModelRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<RoomModelEntity> FindAsync(int id) => await _context.RoomModels
            .FindAsync(id);

        public async Task<RoomModelEntity> FindByNameAsync(string name) => await _context.RoomModels
            .FirstAsync(roomModel => roomModel.Name == name);

        public async Task<List<RoomModelEntity>> FindAllAsync() => await _context.RoomModels
            .ToListAsync();
    }
}
