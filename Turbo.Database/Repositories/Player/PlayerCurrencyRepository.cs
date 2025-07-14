using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Entities.Players;
using Turbo.Database.Context;

namespace Turbo.Database.Repositories.Player;

public class PlayerCurrencyRepository(IEmulatorContext _context) : IPlayerCurrencyRepository
{
    public async Task<PlayerCurrencyEntity> FindAsync(int id)
    {
        return await _context.PlayerCurrencies.FindAsync(id);
    }

    public async Task<List<PlayerCurrencyEntity>> FindAllByPlayerIdAsync(int playerId)
    {
        return await _context.PlayerCurrencies
            .Where(entity => entity.PlayerEntityId == playerId)
            .ToListAsync();
    }
}