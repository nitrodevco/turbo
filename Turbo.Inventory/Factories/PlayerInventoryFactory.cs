using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Players;
using Turbo.Core.Game.Inventory;
using Turbo.Furniture.Factories;
using Turbo.Inventory.Furniture;
using Turbo.Inventory.Badges;
using Turbo.Database.Repositories.Player;
using Turbo.Database.Repositories.Furniture;

namespace Turbo.Inventory.Factories
{
    public class PlayerInventoryFactory : IPlayerInventoryFactory
    {
        private readonly IServiceProvider _provider;

        public PlayerInventoryFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IPlayerInventory Create(IPlayer player)
        {
            var scopeFactory = _provider.GetService<IServiceScopeFactory>();
            var playerFurnitureFactory = _provider.GetService<IPlayerFurnitureFactory>();
            var playerBadgeRepository = _provider.GetService<IPlayerBadgeRepository>();
            var furnitureRepository = _provider.GetService<IFurnitureRepository>();

            if (scopeFactory == null || playerFurnitureFactory == null || playerBadgeRepository == null || furnitureRepository == null) return null;

            var playerFurnitureInventory = new PlayerFurnitureInventory(player, playerFurnitureFactory, furnitureRepository);
            var playerBadgeInventory = new PlayerBadgeInventory(player, playerBadgeRepository);

            return new PlayerInventory(player, playerFurnitureInventory, playerBadgeInventory);
        }
    }
}

