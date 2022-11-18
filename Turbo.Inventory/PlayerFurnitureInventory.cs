using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Inventory;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Repositories.Furniture;
using Turbo.Furniture.Factories;

namespace Turbo.Inventory
{
	public class PlayerFurnitureInventory : IPlayerFurnitureInventory
	{
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

        private async Task LoadFurniture()
        {
            Furniture.Clear();

            // if refreshing, send the inventory invalidate packet

            List<FurnitureEntity> entities;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var furnitureRepository = scope.ServiceProvider.GetService<IFurnitureRepository>();
                entities = await furnitureRepository.FindAllInventoryByPlayerIdAsync(_player.Id);
            }

            foreach (FurnitureEntity furnitureEntity in entities)
            {
                IPlayerFurniture furniture = _playerFurnitureFactory.Create(furnitureEntity);

                Furniture.Add(furniture.Id, furniture);
            }
        }
    }
}

