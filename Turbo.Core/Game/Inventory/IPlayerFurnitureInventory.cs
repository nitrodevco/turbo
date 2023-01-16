using System;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Game.Furniture;

namespace Turbo.Core.Game.Inventory
{
    public interface IPlayerFurnitureInventory : IAsyncInitialisable, IAsyncDisposable
    {
        public IPlayerFurnitureContainer Furniture { get; }
        public IPlayerFurniture GetFurniture(int id);
        public void AddFurnitureFromRoom(IRoomFurniture roomFurniture);
        public void SendFurnitureToSession(ISession session);
    }
}

