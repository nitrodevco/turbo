using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Database.Context;
using Turbo.Database.Entities.Players;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Database.Repositories.Player
{
    public class PlayerCurrencyRepository : IPlayerCurrencyRepository
    {
        private readonly IEmulatorContext _context;

        public PlayerCurrencyRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<PlayerCurrencyEntity> FindAsync(int id) => await _context.PlayerCurrencies.FindAsync(id);

        public async Task<List<PlayerCurrencyEntity>> FindAllByPlayerIdAsync(int playerId) => await _context.PlayerCurrencies
            .Where(entity => entity.PlayerEntityId == playerId)
            .ToListAsync();
    }
}
