using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Players;

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

            return new Player(logger, playerManager, playerEntity);
        }
    }
}
