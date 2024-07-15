using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Players;
using Turbo.Core.Database.Dtos;

namespace Turbo.Database.Repositories.Player
{
    public class PlayerBadgeRepository(IEmulatorContext _context) : IPlayerBadgeRepository
    {
        public async Task<PlayerBadgeEntity> FindAsync(int id) => await _context.PlayerBadges.FindAsync(id);

        public async Task<List<PlayerBadgeEntity>> FindAllByPlayerIdAsync(int playerId) => await _context.PlayerBadges
            .Where(entity => entity.PlayerEntityId == playerId)
            .ToListAsync();

        public async Task<List<PlayerBadgeDto>> FindActiveByPlayerIdAsync(int playerId) => await _context.PlayerBadges
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
