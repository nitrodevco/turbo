using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Core.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room;

public class RoomRepository(IEmulatorContext _context) : IRoomRepository
{
    public async Task<RoomEntity> FindAsync(int id)
    {
        return await _context.Rooms
            .FirstOrDefaultAsync(room => room.Id == id);
    }
    
    public async Task<List<RoomEntity>> GetRoomsOrderedByPopularityAsync()
    {
        return await _context.Rooms
            .OrderByDescending(room => room.UsersNow)
            .ToListAsync();
    }
}