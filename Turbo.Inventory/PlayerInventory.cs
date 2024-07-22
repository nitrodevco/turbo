using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;

namespace Turbo.Inventory;

public class PlayerInventory(
    IPlayer _player,
    IPlayerFurnitureInventory _playerFurnitureInventory,
    IPlayerBadgeInventory _playerBadgeInventory) : Component, IPlayerInventory
{
    public IPlayer Player { get; } = _player;
    public IPlayerFurnitureInventory FurnitureInventory { get; } = _playerFurnitureInventory;
    public IPlayerBadgeInventory BadgeInventory { get; } = _playerBadgeInventory;
    public IUnseenItemsManager UnseenItemsManager { get; } = new UnseenItemsManager(_player);

    protected override async Task OnInit()
    {
        await BadgeInventory.InitAsync();
        await FurnitureInventory.InitAsync();
    }

    protected override async Task OnDispose()
    {
        await BadgeInventory.DisposeAsync();
        await FurnitureInventory.DisposeAsync();
    }
}