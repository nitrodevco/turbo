using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities;
using Turbo.Database.Entities.Players;
using Turbo.Database.Repositories.Player;

namespace Turbo.Database.Repositories.Player
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IEmulatorContext _context;

        public PlayerRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<PlayerEntity> FindAsync(int id) => await _context.Players.FindAsync(id);
    }
}
