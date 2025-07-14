using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Entities.Room;
using Turbo.Database.Context;

namespace Turbo.Database.Repositories.Room;

public class RoomChatlogRepository(IEmulatorContext _context) : IRoomChatlogRepository
{
    public async Task<RoomChatlogEntity> FindAsync(int id)
    {
        return await _context.Chatlogs
            .FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task<List<RoomChatlogEntity>> FindAllByRoomIdAsync(int roomId)
    {
        return await _context.Chatlogs
            .Where(entity => entity.RoomEntityId == roomId)
            .ToListAsync();
    }

    public async Task<bool> AddRoomChatlogAsync(int roomId, int playerId, string message, int? targetPlayerId = null)
    {
        var entity = new RoomChatlogEntity
        {
            RoomEntityId = roomId,
            PlayerEntityId = playerId,
            TargetPlayerEntityId = targetPlayerId,
            Message = message
        };

        _context.Add(entity);

        await _context.SaveChangesAsync();

        return true;
    }
}