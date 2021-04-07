using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room
{
    public class RoomRightRepository : IRoomRightRepository
    {
        private readonly IEmulatorContext _context;

        public RoomRightRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<RoomRightEntity> FindAsync(int id) => await _context.RoomRights
            .FirstOrDefaultAsync(entity => entity.Id == id);

        public async Task<List<RoomRightEntity>> FindAllByRoomIdAsync(int roomId) => await _context.RoomRights
            .Where(entity => entity.RoomEntityId == roomId)
            .ToListAsync();
    }
}
