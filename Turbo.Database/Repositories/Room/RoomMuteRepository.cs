using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room
{
    public class RoomMuteRepository : IRoomMuteRepository
    {
        private readonly IEmulatorContext _context;

        public RoomMuteRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<RoomMuteEntity> FindAsync(int id) => await _context.RoomMutes
            .FirstOrDefaultAsync(entity => entity.Id == id);

        public async Task<List<RoomMuteEntity>> FindAllByRoomIdAsync(int roomId) => await _context.RoomMutes
            .Where(entity => entity.RoomEntityId == roomId)
            .ToListAsync();

        public async Task<bool> RemoveMuteEntityAsync(RoomMuteEntity entity)
        {
            if (entity == null) return false;

            _context.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}