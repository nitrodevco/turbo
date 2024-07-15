using System.Threading.Tasks;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player;

public interface IPlayerSettingsRepository : IBaseRepository<PlayerSettingsEntity>
{
    Task<PlayerSettingsEntity> FindAsync(int id);
    Task<PlayerSettingsEntity> FindByPlayerIdAsync(int playerId);
    Task SaveSettingsAsync(PlayerSettingsEntity settings);
    Task UpdateAsync(PlayerSettingsEntity entity);
}