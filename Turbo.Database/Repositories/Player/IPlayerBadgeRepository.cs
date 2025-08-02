using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player;

public interface IPlayerBadgeRepository : IBaseRepository<PlayerBadgeEntity>
{
    public Task<List<PlayerBadgeEntity>> FindAllByPlayerIdAsync(int playerId);
    public Task<List<PlayerBadgeDto>> FindActiveByPlayerIdAsync(int playerId);
}