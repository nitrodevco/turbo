using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Players;
using Turbo.Database.Repositories.Player;

namespace Turbo.Players
{
    public class PlayerWallet(
        IPlayer _player,
        IServiceScopeFactory _serviceScopeFactory) : Component, IPlayerWallet
    {
        public IDictionary<int, int> Wallet { get; private set; } = new Dictionary<int, int>();

        protected override async Task OnInit()
        {
            await LoadCurrencies();
        }

        protected override async Task OnDispose()
        {
        }

        private async Task LoadCurrencies()
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var currencyRepository = scope.ServiceProvider.GetService<IPlayerCurrencyRepository>();

            List<PlayerCurrencyEntity> entities = await currencyRepository.FindAllByPlayerIdAsync(_player.Id);

            if (entities != null)
            {
                foreach (var currencyEntity in entities)
                {
                    Wallet.Add(currencyEntity.Type, currencyEntity.Amount);
                }
            }
        }
    }
}
