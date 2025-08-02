using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player;

public interface IPlayerChatStyleRepository : IBaseRepository<PlayerChatStyleEntity>
{
    public Task<List<PlayerChatStyleEntity>> FindAllAsync();
}