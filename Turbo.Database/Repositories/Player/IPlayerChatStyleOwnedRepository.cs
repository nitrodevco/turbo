using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player;

public interface IPlayerChatStyleOwnedRepository : IBaseRepository<PlayerChatStyleOwnedEntity>
{
    public Task<List<PlayerChatStyleOwnedEntity>> FindByPlayerIdAsync(int playerId);
    public Task AddAsync(PlayerChatStyleOwnedEntity entity);
}