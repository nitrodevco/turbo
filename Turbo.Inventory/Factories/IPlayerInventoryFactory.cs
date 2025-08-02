using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Players;

namespace Turbo.Inventory.Factories;

public interface IPlayerInventoryFactory
{
    public IPlayerInventory Create(IPlayer player);
}