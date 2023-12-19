using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Players;
using Turbo.Inventory.Factories;

namespace Turbo.Players.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IServiceProvider _provider;

        public PlayerFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IPlayer Create(PlayerEntity playerEntity)
        {
            ILogger<IPlayer> logger = _provider.GetService<ILogger<Player>>();
            IPlayerManager playerManager = _provider.GetService<IPlayerManager>();
            IPlayerInventoryFactory playerInventoryFactory = _provider.GetService<IPlayerInventoryFactory>();
            IServiceScopeFactory scopeFactory = _provider.GetService<IServiceScopeFactory>();

            return new Player(logger, playerManager, CreatePlayerDetails(playerEntity), playerInventoryFactory, scopeFactory);
        }

        public IPlayerDetails CreatePlayerDetails(PlayerEntity playerEntity)
        {
            return new PlayerDetails(playerEntity);
        }
    }
}
