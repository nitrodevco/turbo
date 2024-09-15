using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Database.Factories;
using Turbo.Core.Database.Factories.Players;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Players;
using Turbo.Inventory.Badges;
using Turbo.Inventory.Furniture;

namespace Turbo.Inventory.Factories;

public class PlayerInventoryFactory(IServiceProvider _provider) : IPlayerInventoryFactory
{
    public IPlayerInventory Create(IPlayer player)
    {
        return ActivatorUtilities.CreateInstance<PlayerInventory>(_provider, player,
            ActivatorUtilities.CreateInstance<PlayerFurnitureInventory>(_provider, player),
            ActivatorUtilities.CreateInstance<PlayerBadgeInventory>(_provider, player));
    }
}