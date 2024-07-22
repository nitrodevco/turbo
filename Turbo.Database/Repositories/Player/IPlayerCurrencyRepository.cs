using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player;

public interface IPlayerCurrencyRepository : IBaseRepository<PlayerCurrencyEntity>
{
    public Task<List<PlayerCurrencyEntity>> FindAllByPlayerIdAsync(int playerId);
}