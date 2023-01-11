using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Players;
using Turbo.Inventory.Factories;

namespace Turbo.Inventory
{
    public class PlayerInventory : IPlayerInventory
    {
        public IPlayer Player { get; private set; }
        public IPlayerFurnitureInventory FurnitureInventory { get; private set; }

        public bool IsInitialized { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool IsDisposing { get; private set; }

        public PlayerInventory(
            IPlayer player,
            IPlayerFurnitureInventory playerFurnitureInventory)
        {
            Player = player;
            FurnitureInventory = playerFurnitureInventory;
        }

        public async ValueTask InitAsync()
        {
            if (IsInitialized) return;

            if (FurnitureInventory != null) await FurnitureInventory.InitAsync();

            IsInitialized = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (IsDisposing) return;

            IsDisposing = true;

            if (FurnitureInventory != null) await FurnitureInventory.DisposeAsync();

            IsDisposed = true;
        }
    }
}

