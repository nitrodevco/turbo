using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;
using Turbo.Database.Repositories.Player;

namespace Turbo.Players;

public class PlayerWallet(
    IPlayer _player,
    IServiceScopeFactory _serviceScopeFactory) : Component, IPlayerWallet
{
    protected override async Task OnInit()
    {
        await LoadCurrencies();
    }

    protected override Task OnDispose()
    {
        return Task.CompletedTask;
    }

    private async Task LoadCurrencies()
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var currencyRepository = scope.ServiceProvider.GetRequiredService<IPlayerCurrencyRepository>();

        var entities = await currencyRepository.FindAllByPlayerIdAsync(_player.Id);

        if (entities is not null)
            foreach (var currencyEntity in entities)
            {
            }
    }
}