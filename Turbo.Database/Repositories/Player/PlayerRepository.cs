using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Players;
using Turbo.Core.Database.Dtos;

namespace Turbo.Database.Repositories.Player
{
    public class PlayerRepository(IEmulatorContext _context) : IPlayerRepository
    {
        public async Task<PlayerEntity> FindAsync(int id) => await _context.Players
            .FindAsync(id);

        public async Task<PlayerUsernameDto> FindUsernameAsync(int id) => await _context.Players
            .Where(player => id == player.Id)
            .Select(player => new PlayerUsernameDto
            {
                Id = player.Id,
                Name = player.Name
            })
            .FirstOrDefaultAsync();

        public async Task<IList<PlayerUsernameDto>> FindUsernamesAsync(IList<int> ids) => await _context.Players
            .Where(player => ids.Any(id => id == player.Id))
            .Select(player => new PlayerUsernameDto
            {
                Id = player.Id,
                Name = player.Name
            })
            .ToListAsync();
    }
}
