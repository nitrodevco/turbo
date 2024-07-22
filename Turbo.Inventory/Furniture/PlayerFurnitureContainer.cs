using System.Collections.Concurrent;
using Turbo.Core.Game.Inventory;

namespace Turbo.Inventory.Furniture;

public class PlayerFurnitureContainer(Action<IPlayerFurniture> _onRemove) : IPlayerFurnitureContainer
{
    public ConcurrentDictionary<int, IPlayerFurniture> PlayerFurniture { get; } = new();

    public IPlayerFurniture? GetPlayerFurniture(int id)
    {
        if (id > 0 && PlayerFurniture.TryGetValue(id, out var playerFurniture)) return playerFurniture;

        return null;
    }

    public bool AddFurniture(IPlayerFurniture playerFurniture)
    {
        if (playerFurniture == null) return false;

        return PlayerFurniture.TryAdd(playerFurniture.Id, playerFurniture);
    }

    public void RemoveFurniture(params IPlayerFurniture[] playerFurnitures)
    {
        foreach (var playerFurniture in playerFurnitures)
        {
            if (playerFurniture == null) continue;

            if (!PlayerFurniture.TryRemove(
                    new KeyValuePair<int, IPlayerFurniture>(playerFurniture.Id, playerFurniture))) continue;

            _onRemove?.Invoke(playerFurniture);
        }
    }

    public void RemoveAllFurniture(params int[] ids)
    {
        foreach (var id in ids)
        {
            var furniture = GetPlayerFurniture(id);

            if (furniture == null) continue;

            RemoveFurniture(furniture);
        }
    }
}