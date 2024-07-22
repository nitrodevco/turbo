using System.Collections.Concurrent;

namespace Turbo.Core.Game.Inventory;

public interface IPlayerFurnitureContainer
{
    public ConcurrentDictionary<int, IPlayerFurniture> PlayerFurniture { get; }
    public IPlayerFurniture? GetPlayerFurniture(int id);
    public bool AddFurniture(IPlayerFurniture playerFurniture);
    public void RemoveFurniture(params IPlayerFurniture[] playerFurnitures);
    public void RemoveAllFurniture(params int[] ids);
}