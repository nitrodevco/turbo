using Microsoft.Extensions.DependencyInjection;
using System;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Players;
using Turbo.Inventory.Factories;

namespace Turbo.Players.Factories
{
    public class PlayerFactory(
        IPlayerInventoryFactory _playerInventoryFactory,
        IServiceScopeFactory _serviceScopeFactory,
        IServiceProvider _provider) : IPlayerFactory
    {
        public IPlayer Create(PlayerEntity playerEntity)
        {
            var player = ActivatorUtilities.CreateInstance<Player>(_provider, CreatePlayerDetails(playerEntity));
            var inventory = _playerInventoryFactory.Create(player);
            var wallet = new PlayerWallet(player, _serviceScopeFactory);
            var settings = new PlayerSettings(player, _serviceScopeFactory);

            player.SetInventory(inventory);
            player.SetWallet(wallet);
            player.SetSettings(settings);

            return player;
        }

        public IPlayerDetails CreatePlayerDetails(PlayerEntity playerEntity)
        {
            return new PlayerDetails(playerEntity);
        }
    }
}
