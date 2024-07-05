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
    public class PlayerInventoryFactory(IServiceProvider provider) : IPlayerInventoryFactory
    {
        private readonly IServiceProvider _provider = provider;

        public IPlayerInventory Create(IPlayer player)
        {
            return ActivatorUtilities.CreateInstance<PlayerInventory>(_provider, player, ActivatorUtilities.CreateInstance<PlayerFurnitureInventory>(_provider, player), ActivatorUtilities.CreateInstance<PlayerBadgeInventory>(_provider, player));
        }
    }
}

