using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Dtos;
using Turbo.Database.Context;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player;

public class PlayerBadgeRepository(IEmulatorContext _context) : IPlayerBadgeRepository
{
    public async Task<PlayerBadgeEntity> FindAsync(int id)
    {
        return await _context.PlayerBadges.FindAsync(id);
    }

    public async Task<List<PlayerBadgeEntity>> FindAllByPlayerIdAsync(int playerId)
    {
        return await _context.PlayerBadges
            .Where(entity => entity.PlayerEntityId == playerId)
            .ToListAsync();
    }

    public async Task<List<PlayerBadgeDto>> FindActiveByPlayerIdAsync(int playerId)
    {
        return await _context.PlayerBadges
            .Where(entity => entity.PlayerEntityId == playerId && entity.SlotId != null)
            .Select(entity => new PlayerBadgeDto
            {
                Id = entity.Id,
                BadgeCode = entity.BadgeCode,
                SlotId = entity.SlotId
            })
            .ToListAsync();
    }
}