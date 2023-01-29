using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Players;
using Turbo.Core.Utilities;
using Turbo.Inventory.Factories;

namespace Turbo.Inventory
{
    public class PlayerInventory : Component, IPlayerInventory
    {
        public IPlayer Player { get; private set; }
        public IPlayerFurnitureInventory FurnitureInventory { get; private set; }
        public IPlayerBadgeInventory BadgeInventory { get; private set; }
        public IUnseenItemsManager UnseenItemsManager { get; private set; }

        public PlayerInventory(
            IPlayer player,
            IPlayerFurnitureInventory playerFurnitureInventory,
            IPlayerBadgeInventory playerBadgeInventory)
        {
            Player = player;
            FurnitureInventory = playerFurnitureInventory;
            BadgeInventory = playerBadgeInventory;
            UnseenItemsManager = new UnseenItemsManager(player);
        }

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
}

