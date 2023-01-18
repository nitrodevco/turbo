using System;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Game.Inventory
{
    public interface IPlayerInventory : IAsyncInitialisable, IAsyncDisposable
    {
        public IPlayer Player { get; }
        public IPlayerFurnitureInventory FurnitureInventory { get; }
        public IPlayerBadgeInventory BadgeInventory { get; }
        public IUnseenItemsManager UnseenItemsManager { get; }

    }
}

