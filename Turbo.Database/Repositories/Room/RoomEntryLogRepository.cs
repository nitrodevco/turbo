using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Entities.Room;
using Turbo.Database.Context;

namespace Turbo.Database.Repositories.Room;
public class RoomEntryLogRepository(IEmulatorContext _context) : IRoomEntryLogRepository
{
    public async Task<RoomEntryLogEntity> FindAsync(int id)
    {
        return await _context.RoomEntryLogs
            .FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task<List<RoomEntity>> GetFrequentRoomsByPlayerIdAsync(int playerId, int frequency, int maximumCount)
    {
        return await _context.RoomEntryLogs
            .Where(entity => entity.PlayerEntityId == playerId)
            .GroupBy(entity => entity.RoomEntityId)
            .Where(group => group.Count() >= frequency)
            .OrderByDescending(group => group.Max(entity => entity.CreatedAt))
            .Select(group => group.First())
            .Select(log => log.RoomEntity)
            .Take(maximumCount)
            .ToListAsync();
    }

    public async Task<List<RoomEntity>> GetLatestByPlayerIdAsync(int playerId, int maximumCount)
    {
        return await _context.RoomEntryLogs
            .Where(entity => entity.PlayerEntityId == playerId)
            .OrderByDescending(entity => entity.CreatedAt)
            .Select(log => log.RoomEntity)
            .Distinct()
            .Take(maximumCount)
            .ToListAsync();
    }

    public async Task<bool> AddRoomEntryLogAsync(int roomId, int playerId)
    {
        var entity = new RoomEntryLogEntity
        {
            RoomEntityId = roomId,
            PlayerEntityId = playerId
        };

        _context.Add(entity);

        await _context.SaveChangesAsync();

        return true;
    }
}
