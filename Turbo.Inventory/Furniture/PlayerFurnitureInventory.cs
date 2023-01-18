using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Inventory;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Repositories.Furniture;
using Turbo.Furniture.Factories;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Packets.Outgoing.Inventory.Furni;
using Turbo.Core.Game.Inventory.Constants;
using Turbo.Core.Game;

namespace Turbo.Inventory.Furniture
{
    public class PlayerFurnitureInventory : IPlayerFurnitureInventory
    {
        private readonly IPlayer _player;
        private readonly IPlayerFurnitureFactory _playerFurnitureFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IPlayerFurnitureContainer Furniture { get; private set; }

        private bool _requested;

        public PlayerFurnitureInventory(
            IPlayer player,
            IPlayerFurnitureFactory playerFurnitureFactory,
            IServiceScopeFactory serviceScopeFactory)
        {
            _player = player;
            _playerFurnitureFactory = playerFurnitureFactory;
            _serviceScopeFactory = serviceScopeFactory;

            Furniture = new PlayerFurnitureContainer(RemoveFurniture);
        }

        public async ValueTask InitAsync()
        {
            await LoadFurniture();
        }

        public async ValueTask DisposeAsync()
        {
            Furniture.PlayerFurniture.Clear();
        }

        public IPlayerFurniture GetFurniture(int id)
        {
            return Furniture.GetPlayerFurniture(id);
        }

        public void AddFurnitureFromRoom(IRoomFurniture roomFurniture)
        {
            if (roomFurniture == null) return;

            var playerFurniture = _playerFurnitureFactory.CreateFromRoomFurniture(Furniture, roomFurniture, _player.Id);

            if (playerFurniture == null) return;

            Furniture.AddFurniture(playerFurniture);

            _player.PlayerInventory?.UnseenItemsManager?.Add(UnseenItemCategory.Furni, playerFurniture.Id);

            if (!_requested) return;

            _player.Session?.Send(new FurniListAddOrUpdateMessage
            {
                Furniture = playerFurniture
            });
        }

        public void RemoveFurniture(IPlayerFurniture playerFurniture)
        {
            if ((playerFurniture == null) || playerFurniture.Disposed) return;

            Furniture.RemoveFurniture(playerFurniture);

            playerFurniture.Dispose();

            if (!_requested) return;

            _player.Session?.Send(new FurniListRemoveMessage
            {
                ItemId = playerFurniture.Id
            });
        }

        public void SendFurnitureToSession(ISession session)
        {
            List<IPlayerFurniture> playerFurnitures = new();

            var totalFragments = (int)Math.Ceiling((double)Furniture.PlayerFurniture.Count / DefaultSettings.FurniPerFragment);

            if (totalFragments == 0) totalFragments = 1;

            int currentFragment = 0;
            int count = 0;

            foreach (var playerFurniture in Furniture.PlayerFurniture.Values)
            {
                playerFurnitures.Add(playerFurniture);

                count++;

                if (count == DefaultSettings.FurniPerFragment)
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

            _requested = true;
        }

        private async Task LoadFurniture()
        {
            Furniture.PlayerFurniture.Clear();

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
                foreach (var furnitureEntity in entities)
                {
                    var playerFurniture = _playerFurnitureFactory.Create(Furniture, furnitureEntity);

                    Furniture.AddFurniture(playerFurniture);
                }
            }

            if (_requested)
            {
                _player.Session?.Send(new FurniListInvalidateMessage());
            }
        }
    }
}

