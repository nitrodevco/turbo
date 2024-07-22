using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Inventory;

public interface IPlayerInventory : IComponent
{
    public IPlayer Player { get; }
    public IPlayerFurnitureInventory FurnitureInventory { get; }
    public IPlayerBadgeInventory BadgeInventory { get; }
    public IUnseenItemsManager UnseenItemsManager { get; }
}