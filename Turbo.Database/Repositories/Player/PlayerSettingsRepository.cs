using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player;

public class PlayerSettingsRepository(IEmulatorContext context) : IPlayerSettingsRepository
{
    public async Task<PlayerSettingsEntity> FindAsync(int id)
    {
        return await context.PlayerSettings
            .FirstOrDefaultAsync(settings => settings.Id == id);
    }
    
    public async Task<PlayerSettingsEntity> FindByPlayerIdAsync(int playerId)
    {
        return await context.PlayerSettings
            .FirstOrDefaultAsync(settings => settings.PlayerEntityId == playerId);
    }

    public async Task SaveSettingsAsync(PlayerSettingsEntity settings)
    {
        context.PlayerSettings.Update(settings);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(PlayerSettingsEntity entity)
    {
        context.PlayerSettings.Update(entity);
        await context.SaveChangesAsync();
    }
}