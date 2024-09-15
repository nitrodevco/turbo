using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Database.Factories.Players;

public interface IPlayerInventoryFactory
{
    public IPlayerInventory Create(IPlayer player);
}