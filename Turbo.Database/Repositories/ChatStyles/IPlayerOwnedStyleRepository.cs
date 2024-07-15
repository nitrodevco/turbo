using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.ChatStyles;

namespace Turbo.Database.Repositories.ChatStyles
{
    public interface IPlayerOwnedStyleRepository : IBaseRepository<PlayerOwnedStyleEntity>
    {
        Task<List<PlayerOwnedStyleEntity>> FindByPlayerIdAsync(int playerId);
        Task AddAsync(PlayerOwnedStyleEntity entity);
    }
}