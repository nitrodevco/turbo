using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Players;
using Turbo.Core.Game.Inventory;
using Turbo.Furniture.Factories;

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
            IServiceScopeFactory scopeFactory = _provider.GetService<IServiceScopeFactory>();
            IPlayerFurnitureFactory playerFurnitureFactory = _provider.GetService<IPlayerFurnitureFactory>();

            IPlayerFurnitureInventory playerFurnitureInventory = new PlayerFurnitureInventory(player, playerFurnitureFactory, scopeFactory);

            return new PlayerInventory(player, playerFurnitureInventory);
        }
    }
}

