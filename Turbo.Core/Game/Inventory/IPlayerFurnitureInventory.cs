using System;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Core.Game.Inventory
{
    public interface IPlayerFurnitureInventory : IAsyncInitialisable, IAsyncDisposable
    {
        public IPlayerFurnitureContainer Furniture { get; }
        public IPlayerFurniture GetFurniture(int id);
        public void AddFurnitureFromRoom(IRoomFurniture roomFurniture);
        public void RemoveFurniture(IPlayerFurniture playerFurniture);
        public ValueTask GiveFurniture(int definitionId);
        public void SendFurnitureToSession(ISession session);
    }
}

