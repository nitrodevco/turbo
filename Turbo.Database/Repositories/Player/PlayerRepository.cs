using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Players;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Database.Repositories.Player
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IEmulatorContext _context;

        public PlayerRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<PlayerEntity> FindAsync(int id) => await _context.Players
            .FirstOrDefaultAsync(player => player.Id == id);
    }
}
