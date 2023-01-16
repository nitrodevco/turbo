using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Database.Entities.Furniture;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Inventory;

namespace Turbo.Furniture.Factories
{
    public class FurnitureFactory : IFurnitureFactory
    {
        private readonly IServiceProvider _provider;

        public FurnitureFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IFurnitureDefinition GetFurnitureDefinition(int id)
        {
            var furnitureManager = _provider.GetService<IFurnitureManager>();

            return furnitureManager.GetFurnitureDefinition(id);
        }

        public IRoomFloorFurniture CreateFloorFurniture(IRoomFurnitureManager roomFurnitureManager, FurnitureEntity furnitureEntity)
        {
            var furnitureManager = _provider.GetService<IFurnitureManager>();

            var furnitureDefinition = furnitureManager.GetFurnitureDefinition(furnitureEntity.FurnitureDefinitionEntityId);

            if (furnitureDefinition == null) return null;

            var logger = _provider.GetService<ILogger<RoomFloorFurniture>>();
            IServiceScopeFactory scopeFactory = _provider.GetService<IServiceScopeFactory>();

            return new RoomFloorFurniture(logger, roomFurnitureManager, furnitureManager, furnitureEntity, furnitureDefinition);
        }

        public IRoomFloorFurniture CreateFloorFurnitureFromPlayerFurniture(IRoomFurnitureManager roomFurnitureManager, IPlayerFurniture playerFurniture)
        {
            if (playerFurniture is PlayerFurniture furniture) return CreateFloorFurniture(roomFurnitureManager, furniture.FurnitureEntity);

            return null;
        }

        public IRoomWallFurniture CreateWallFurniture(IRoomFurnitureManager roomFurnitureManager, FurnitureEntity furnitureEntity)
        {
            var furnitureManager = _provider.GetService<IFurnitureManager>();

            var furnitureDefinition = furnitureManager.GetFurnitureDefinition(furnitureEntity.FurnitureDefinitionEntityId);

            if (furnitureDefinition == null) return null;

            var logger = _provider.GetService<ILogger<RoomWallFurniture>>();
            IServiceScopeFactory scopeFactory = _provider.GetService<IServiceScopeFactory>();

            return new RoomWallFurniture(logger, roomFurnitureManager, furnitureManager, furnitureEntity, furnitureDefinition);
        }

        public IRoomWallFurniture CreateWallFurnitureFromPlayerFurniture(IRoomFurnitureManager roomFurnitureManager, IPlayerFurniture playerFurniture)
        {
            if (playerFurniture is PlayerFurniture furniture) return CreateWallFurniture(roomFurnitureManager, furniture.FurnitureEntity);

            return null;
        }
    }
}
