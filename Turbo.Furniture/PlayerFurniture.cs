using System;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture
{
	public class PlayerFurniture : IPlayerFurniture
	{
        public ILogger<IFurniture> Logger { get; private set; }

        private readonly FurnitureEntity _furnitureEntity;

        public PlayerFurniture(
            ILogger<IFurniture> logger,
            FurnitureEntity furnitureEntity,
            IFurnitureDefinition furnitureDefinition)
		{
            Logger = logger;
            _furnitureEntity = furnitureEntity;
		}

        public int Id => _furnitureEntity.Id;

        public int PlayerId => _furnitureEntity.PlayerEntityId;
    }
}

