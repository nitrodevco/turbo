using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Database.Entities.Furniture;
using Turbo.Core.Game.Rooms.Object.Logic;

namespace Turbo.Furniture.Factories
{
    public class PlayerFurnitureFactory : IPlayerFurnitureFactory
    {
        private readonly IServiceProvider _provider;

        public PlayerFurnitureFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IPlayerFurniture Create(IPlayerFurnitureContainer playerFurnitureContainer, FurnitureEntity furnitureEntity)
        {
            IFurnitureManager furnitureManager = _provider.GetService<IFurnitureManager>();

            IFurnitureDefinition furnitureDefinition = furnitureManager.GetFurnitureDefinition(furnitureEntity.FurnitureDefinitionEntityId);

            if (furnitureDefinition == null) return null;

            ILogger<IPlayerFurniture> logger = _provider.GetService<ILogger<PlayerFurniture>>();
            IServiceScopeFactory scopeFactory = _provider.GetService<IServiceScopeFactory>();
            IRoomObjectLogicFactory logicFactory = _provider.GetService<IRoomObjectLogicFactory>();

            var stuffDataKey = logicFactory.GetStuffDataKeyForFurnitureType(furnitureDefinition.Logic);

            return new PlayerFurniture(logger, playerFurnitureContainer, furnitureEntity, furnitureDefinition, stuffDataKey);
        }
    }
}