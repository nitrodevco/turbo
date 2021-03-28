using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Dtos;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player
{
    public interface IPlayerRepository : IBaseRepository<PlayerEntity>
    {
        public Task<IList<PlayerUsernameDto>> FindUsernamesAsync(IList<int> ids);
    }
}
