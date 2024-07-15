using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.ChatStyles;

namespace Turbo.Database.Repositories.ChatStyles
{
    public class PlayerOwnedStyleRepository(IEmulatorContext context) : IPlayerOwnedStyleRepository
    {
        public async Task<PlayerOwnedStyleEntity> FindAsync(int playerId)
        {
            return await context.OwnedChatStyles
                .FirstOrDefaultAsync(e => e.PlayerEntityId == playerId);
        }

        public async Task<List<PlayerOwnedStyleEntity>> FindByPlayerIdAsync(int playerId)
        {
            return await context.OwnedChatStyles
                .Where(e => e.PlayerEntityId == playerId)
                .ToListAsync();
        }

        public async Task AddAsync(PlayerOwnedStyleEntity entity)
        {
            context.OwnedChatStyles.Add(entity);
            await context.SaveChangesAsync();
        }
    }
}