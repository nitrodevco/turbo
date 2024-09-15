using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player;

public interface IPlayerRepository : IBaseRepository<PlayerEntity>
{
    public Task<PlayerUsernameDto> FindUsernameAsync(int id);
    public Task<IList<PlayerUsernameDto>> FindUsernamesAsync(IList<int> ids);
}