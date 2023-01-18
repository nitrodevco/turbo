using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Players;
using Turbo.Inventory.Factories;

namespace Turbo.Inventory
{
    public class PlayerInventory : IPlayerInventory
    {
        public IPlayer Player { get; private set; }
        public IPlayerFurnitureInventory FurnitureInventory { get; private set; }
        public IPlayerBadgeInventory BadgeInventory { get; private set; }
        public IUnseenItemsManager UnseenItemsManager { get; private set; }

        public bool IsInitialized { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool IsDisposing { get; private set; }

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

        public async ValueTask InitAsync()
        {
            if (IsInitialized) return;

            if (BadgeInventory != null) await BadgeInventory.InitAsync();
            if (FurnitureInventory != null) await FurnitureInventory.InitAsync();

            IsInitialized = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (IsDisposing) return;

            IsDisposing = true;

            if (BadgeInventory != null) await BadgeInventory.DisposeAsync();
            if (FurnitureInventory != null) await FurnitureInventory.DisposeAsync();

            IsDisposed = true;
        }
    }
}

