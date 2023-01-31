using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room
{
    public class RoomBanRepository : IRoomBanRepository
    {
        private readonly IEmulatorContext _context;

        public RoomBanRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<RoomBanEntity> FindAsync(int id) => await _context.RoomBans
            .FirstOrDefaultAsync(entity => entity.Id == id);

        public async Task<List<RoomBanEntity>> FindAllByRoomIdAsync(int roomId) => await _context.RoomBans
            .Where(entity => entity.RoomEntityId == roomId)
            .ToListAsync();

        public async Task<bool> RemoveBanEntityAsync(RoomBanEntity entity)
        {
            if (entity == null) return false;

            _context.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}