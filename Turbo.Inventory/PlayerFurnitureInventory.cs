using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Inventory;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Repositories.Furniture;
using Turbo.Furniture.Factories;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Packets.Outgoing.Inventory.Furni;

namespace Turbo.Inventory
{
    public class PlayerFurnitureInventory : IPlayerFurnitureInventory, IPlayerFurnitureContainer
    {
        private static int FurniPerFragment = 100;

        private readonly IPlayer _player;
        private readonly IPlayerFurnitureFactory _playerFurnitureFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IDictionary<int, IPlayerFurniture> Furniture { get; private set; }

        public bool IsInitialized { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool IsDisposing { get; private set; }

        public PlayerFurnitureInventory(
            IPlayer player,
            IPlayerFurnitureFactory playerFurnitureFactory,
            IServiceScopeFactory serviceScopeFactory)
        {
            _player = player;
            _playerFurnitureFactory = playerFurnitureFactory;
            _serviceScopeFactory = serviceScopeFactory;

            Furniture = new Dictionary<int, IPlayerFurniture>();
        }

        public async ValueTask InitAsync()
        {
            if (IsInitialized) return;

            await LoadFurniture();

            IsInitialized = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (IsDisposing) return;

            IsDisposing = true;

            Furniture.Clear();

            IsDisposed = true;
        }

        public IPlayerFurniture? GetFurniture(int id)
        {
            if (id <= 0) return null;

            if (Furniture.TryGetValue(id, out IPlayerFurniture? furniture))
            {
                return furniture;
            }

            return null;
        }

        public void RemoveFurniture(params int[] ids)
        {
            foreach (int id in ids) RemoveFurniture(id);
        }

        public void RemoveFurniture(int id)
        {
            var furniture = GetFurniture(id);

            if (furniture == null) return;

            Furniture.Remove(id);

            furniture.Dispose();
        }

        public void AddFurniture(params IRoomFloorFurniture[] furnitures)
        {

        }

        public void RemoveAllFurniture()
        {
            foreach (int id in Furniture.Keys) RemoveFurniture(id);
        }

        public void SendFurnitureToSession(ISession session)
        {
            List<IPlayerFurniture> playerFurnitures = new();

            int totalFragments = Furniture.Count % FurniPerFragment;
            int currentFragment = 0;
            int count = 0;

            foreach (IPlayerFurniture playerFurniture in Furniture.Values)
            {
                playerFurnitures.Add(playerFurniture);

                count++;

                if (count == FurniPerFragment)
                {
                    session.Send(new FurniListMessage
                    {
                        TotalFragments = totalFragments,
                        CurrentFragment = currentFragment,
                        Furniture = playerFurnitures
                    });

                    playerFurnitures.Clear();
                    count = 0;
                    currentFragment++;
                }
            }

            if (count <= 0) return;

            session.Send(new FurniListMessage
            {
                TotalFragments = totalFragments,
                CurrentFragment = currentFragment,
                Furniture = playerFurnitures
            });
        }

        private async Task LoadFurniture()
        {
            Furniture.Clear();

            List<FurnitureEntity> entities = new();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var furnitureRepository = scope.ServiceProvider.GetService<IFurnitureRepository>();

                if (furnitureRepository != null)
                {
                    entities = await furnitureRepository.FindAllInventoryByPlayerIdAsync(_player.Id);
                }
            }

            if (entities != null)
            {
                foreach (FurnitureEntity furnitureEntity in entities)
                {
                    IPlayerFurniture playerFurniture = _playerFurnitureFactory.Create(this, furnitureEntity);

                    Furniture.Add(playerFurniture.Id, playerFurniture);
                }
            }

            if (IsInitialized)
            {
                _player.Session?.Send(new FurniListInvalidateMessage());
            }
        }
    }
}

