using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Players;
using Turbo.Inventory.Factories;

namespace Turbo.Players.Factories
{
    public class PlayerFactory(IServiceProvider provider) : IPlayerFactory
    {
        private readonly IServiceProvider _provider = provider;

        public IPlayer Create(PlayerEntity playerEntity)
        {
            return ActivatorUtilities.CreateInstance<Player>(_provider, CreatePlayerDetails(playerEntity));
        }

        public IPlayerDetails CreatePlayerDetails(PlayerEntity playerEntity)
        {
            return new PlayerDetails(playerEntity);
        }
    }
}
