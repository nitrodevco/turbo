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
    public class PlayerWallet : Component, IPlayerWallet
    {
        private readonly IPlayer _player;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PlayerWallet(IPlayer player, IServiceScopeFactory serviceScopeFactory)
        {
            _player = player;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task OnInit()
        {
            await LoadCurrencies();
        }

        protected override async Task OnDispose()
        {
        }

        private async Task LoadCurrencies()
        {
            List<PlayerCurrencyEntity> entities = new();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var currencyRepository = scope.ServiceProvider.GetService<IPlayerCurrencyRepository>();

                if (currencyRepository != null)
                {
                    entities = await currencyRepository.FindAllByPlayerIdAsync(_player.Id);
                }
            }

            if (entities != null)
            {
                foreach (var currencyEntity in entities)
                {
                }
            }
        }
    }
}
