using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Entities.Players;
using Turbo.Database.Repositories.Furniture;
using Turbo.Database.Repositories.Player;
using Turbo.Packets.Outgoing.Inventory.Furni;

namespace Turbo.Players
{
    public class PlayerWallet(
        IPlayer _player,
        IServiceScopeFactory _serviceScopeFactory) : Component, IPlayerWallet
    {
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
                }
            }
        }
    }
}
