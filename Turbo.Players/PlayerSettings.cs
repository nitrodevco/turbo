using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Players;
using Turbo.Database.Repositories.ChatStyles;
using Turbo.Database.Repositories.Player;

public class PlayerSettings(
    IPlayer _player,
    IServiceScopeFactory _serviceScopeFactory) : Component, IPlayerSettings
{
    public int ChatStyle { get; set; }
    public List<int> OwnedChatStyles { get; set; } = new List<int>();

    protected override async Task OnInit()
    {
        await LoadSettings();
        await LoadOwnedChatStyles();
        EnsureDefaultChatStyle();
    }

    protected override Task OnDispose()
    {
        return Task.CompletedTask;
    }

    public async Task SaveSettings()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var playerSettingsRepository = scope.ServiceProvider.GetService<IPlayerSettingsRepository>();

        if (playerSettingsRepository != null)
        {
            var existingSettings = await playerSettingsRepository.FindByPlayerIdAsync(_player.Id);
            if (existingSettings != null)
            {
                existingSettings.ChatStyle = ChatStyle;
                await playerSettingsRepository.UpdateAsync(existingSettings);
            }
            else
            {
                var settingsEntity = new PlayerSettingsEntity
                {
                    PlayerEntityId = _player.Id,
                    ChatStyle = ChatStyle
                };
                await playerSettingsRepository.SaveSettingsAsync(settingsEntity);
            }
        }
    }

    public async Task LoadSettings()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var playerSettingsRepository = scope.ServiceProvider.GetService<IPlayerSettingsRepository>();

        if (playerSettingsRepository != null)
        {
            var settingsEntity = await playerSettingsRepository.FindByPlayerIdAsync(_player.Id);

            if (settingsEntity != null)
            {
                ChatStyle = settingsEntity.ChatStyle;
            }
        }
    }

    public async Task LoadOwnedChatStyles()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var playerOwnedStyleRepository = scope.ServiceProvider.GetService<IPlayerOwnedStyleRepository>();

        if (playerOwnedStyleRepository != null)
        {
            var ownedStyles = await playerOwnedStyleRepository.FindByPlayerIdAsync(_player.Id);

            OwnedChatStyles = ownedStyles.Select(os => os.ChatStyleId).ToList();
        }

        EnsureDefaultChatStyle();
    }

    public void EnsureDefaultChatStyle()
    {
        if (!OwnedChatStyles.Contains(0))
        {
            OwnedChatStyles.Add(0);
        }

        if (ChatStyle != 0 && !OwnedChatStyles.Contains(ChatStyle))
        {
            ChatStyle = 0;
        }
    }
}