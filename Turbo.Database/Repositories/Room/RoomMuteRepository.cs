using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Core.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room;

public class RoomMuteRepository(IEmulatorContext _context) : IRoomMuteRepository
{
    public async Task<RoomMuteEntity> FindAsync(int id)
    {
        return await _context.RoomMutes
            .FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task<List<RoomMuteEntity>> FindAllByRoomIdAsync(int roomId)
    {
        return await _context.RoomMutes
            .Where(entity => entity.RoomEntityId == roomId)
            .ToListAsync();
    }

    public async Task<bool> RemoveMuteEntityAsync(RoomMuteEntity entity)
    {
        if (entity == null) return false;

        _context.Remove(entity);

        await _context.SaveChangesAsync();

        return true;
    }
}