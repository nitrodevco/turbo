using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Dtos;
using Turbo.Database.Context;
using Turbo.Core.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player;

public class PlayerRepository(IEmulatorContext _context) : IPlayerRepository
{
    public async Task<PlayerEntity> FindAsync(int id)
    {
        return await _context.Players
            .FindAsync(id);
    }

    public async Task<PlayerUsernameDto> FindUsernameAsync(int id)
    {
        return await _context.Players
            .Where(player => id == player.Id)
            .Select(player => new PlayerUsernameDto
            {
                Id = player.Id,
                Name = player.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IList<PlayerUsernameDto>> FindUsernamesAsync(IList<int> ids)
    {
        return await _context.Players
            .Where(player => ids.Any(id => id == player.Id))
            .Select(player => new PlayerUsernameDto
            {
                Id = player.Id,
                Name = player.Name
            })
            .ToListAsync();
    }

    public async Task<PlayerUsernameDto> FindUserIdAsync(string username)
    {
        return await _context.Players
        .Where(player => username == player.Name)
        .Select(player => new PlayerUsernameDto
        {
            Id = player.Id,
            Name = player.Name
        })
        .FirstOrDefaultAsync();
    }
}