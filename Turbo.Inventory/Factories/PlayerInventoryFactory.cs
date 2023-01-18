using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Players;
using Turbo.Core.Game.Inventory;
using Turbo.Furniture.Factories;
using Turbo.Inventory.Furniture;
using Turbo.Inventory.Badges;
using Turbo.Core.Storage;

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
            var storageQueue = _provider.GetService<IStorageQueue>();

            if (scopeFactory == null || playerFurnitureFactory == null || storageQueue == null) return null;

            var playerFurnitureInventory = new PlayerFurnitureInventory(player, playerFurnitureFactory, scopeFactory);
            var playerBadgeInventory = new PlayerBadgeInventory(player, storageQueue, scopeFactory);

            return new PlayerInventory(player, playerFurnitureInventory, playerBadgeInventory);
        }
    }
}

