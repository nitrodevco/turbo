using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Furniture.Data;
using Turbo.Database.Entities.Furniture;
using Turbo.Furniture.Data;

namespace Turbo.Furniture
{
    public class PlayerFurniture : IPlayerFurniture
    {
        private readonly ILogger<IPlayerFurniture> _logger;

        private readonly IPlayerFurnitureContainer _playerFurnitureContainer;
        private readonly FurnitureEntity _furnitureEntity;

        public IFurnitureDefinition FurnitureDefinition { get; private set; }
        public IStuffData StuffData { get; private set; }

        public PlayerFurniture(
            ILogger<IPlayerFurniture> logger,
            IPlayerFurnitureContainer playerFurnitureContainer,
            FurnitureEntity furnitureEntity,
            IFurnitureDefinition furnitureDefinition,
            StuffDataKey stuffDataKey)
        {
            _logger = logger;
            _playerFurnitureContainer = playerFurnitureContainer;
            _furnitureEntity = furnitureEntity;

            FurnitureDefinition = furnitureDefinition;
            StuffData = StuffDataFactory.CreateStuffDataFromJson((int)stuffDataKey, _furnitureEntity.StuffData);
        }

        public void Dispose()
        {

        }

        public int Id => _furnitureEntity.Id;
    }
}