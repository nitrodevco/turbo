using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Inventory;

public interface IPlayerFurnitureInventory : IComponent
{
    public IPlayerFurnitureContainer Furniture { get; }
    public IPlayerFurniture? GetFurniture(int id);
    public void AddFurnitureFromRoom(IRoomFurniture roomFurniture);
    public void RemoveFurniture(IPlayerFurniture playerFurniture);
    public Task GiveFurniture(int definitionId);
    public void SendFurnitureToSession(ISession session);
}