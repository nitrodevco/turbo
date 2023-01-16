using System.Collections.Concurrent;
using Turbo.Core.Game.Inventory;

namespace Turbo.Inventory
{
    public class PlayerFurnitureContainer : IPlayerFurnitureContainer
    {
        public ConcurrentDictionary<int, IPlayerFurniture> PlayerFurniture { get; private set; }

        private readonly Action<IPlayerFurniture> _onRemove;

        public PlayerFurnitureContainer(Action<IPlayerFurniture> onRemove)
        {
            PlayerFurniture = new ConcurrentDictionary<int, IPlayerFurniture>();
            _onRemove = onRemove;
        }

        public IPlayerFurniture GetPlayerFurniture(int id)
        {
            if ((id > 0) && PlayerFurniture.TryGetValue(id, out var playerFurniture))
            {
                return playerFurniture;
            }

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

                if (!PlayerFurniture.TryRemove(new KeyValuePair<int, IPlayerFurniture>(playerFurniture.Id, playerFurniture))) continue;

                if (_onRemove != null) _onRemove(playerFurniture);
            }
        }

        public void RemoveAllFurniture(params int[] ids)
        {
            foreach (int id in ids)
            {
                RemoveFurniture(GetPlayerFurniture(id));
            }
        }
    }
}