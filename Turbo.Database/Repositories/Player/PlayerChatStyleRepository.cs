using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Entities.Players;
using Turbo.Database.Context;

namespace Turbo.Database.Repositories.ChatStyles;

public class PlayerChatStyleRepository(IEmulatorContext _context) : IPlayerChatStyleRepository
{
    public async Task<PlayerChatStyleEntity> FindAsync(int id)
    {
        return await _context.PlayerChatStyles
            .FindAsync(id);
    }

    public async Task<List<PlayerChatStyleEntity>> FindAllAsync()
    {
        return await _context.PlayerChatStyles
            .ToListAsync();
    }
}