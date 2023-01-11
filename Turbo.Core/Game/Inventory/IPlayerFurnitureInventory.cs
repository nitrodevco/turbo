using System;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Game.Furniture;

namespace Turbo.Core.Game.Inventory
{
    public interface IPlayerFurnitureInventory : IPlayerFurnitureContainer, IAsyncInitialisable, IAsyncDisposable
    {
        public void AddFurniture(params IRoomFloorFurniture[] furnitures);
        public void SendFurnitureToSession(ISession session);
    }
}

