using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room;

public class RoomBanRepository(IEmulatorContext _context) : IRoomBanRepository
{
    public async Task<RoomBanEntity> FindAsync(int id)
    {
        return await _context.RoomBans
            .FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task<List<RoomBanEntity>> FindAllByRoomIdAsync(int roomId)
    {
        return await _context.RoomBans
            .Where(entity => entity.RoomEntityId == roomId)
            .ToListAsync();
    }

    public async Task<bool> BanPlayerIdAsync(int roomId, int playerId, DateTime expiration)
    {
        var entity = await _context.RoomBans.FirstOrDefaultAsync(entity =>
            entity.RoomEntityId == roomId && entity.PlayerEntityId == playerId);

        if (entity != null) return false;

        entity = new RoomBanEntity();

        entity.RoomEntityId = roomId;
        entity.PlayerEntityId = playerId;
        entity.DateExpires = expiration;

        _context.Add(entity);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveBanEntityAsync(RoomBanEntity entity)
    {
        if (entity == null) return false;

        _context.Remove(entity);

        await _context.SaveChangesAsync();

        return true;
    }
}