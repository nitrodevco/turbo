using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture.Factories
{
	public class PlayerFurnitureFactory : IPlayerFurnitureFactory
	{
        private readonly IServiceProvider _provider;

        public PlayerFurnitureFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IPlayerFurniture Create(FurnitureEntity furnitureEntity)
        {
            IFurnitureManager furnitureManager = _provider.GetService<IFurnitureManager>();

            IFurnitureDefinition furnitureDefinition = furnitureManager.GetFurnitureDefinition(furnitureEntity.FurnitureDefinitionEntityId);

            if (furnitureDefinition == null) return null;

            ILogger<IFurniture> logger = _provider.GetService<ILogger<Furniture>>();
            IServiceScopeFactory scopeFactory = _provider.GetService<IServiceScopeFactory>();

            return new PlayerFurniture(logger, furnitureEntity, furnitureDefinition);
        }
	}
}

