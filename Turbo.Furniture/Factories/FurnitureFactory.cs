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
    public class FurnitureFactory(
        IFurnitureManager _furnitureManager,
        IServiceProvider _provider) : IFurnitureFactory
    {
        public IFurnitureDefinition GetFurnitureDefinition(int id)
        {
            return _furnitureManager.GetFurnitureDefinition(id);
        }

        public IRoomFloorFurniture CreateFloorFurniture(IRoomFurnitureManager roomFurnitureManager, FurnitureEntity furnitureEntity)
        {
            var furnitureDefinition = _furnitureManager.GetFurnitureDefinition(furnitureEntity.FurnitureDefinitionEntityId);

            if (furnitureDefinition == null) return null;

            return ActivatorUtilities.CreateInstance<RoomFloorFurniture>(_provider, roomFurnitureManager, furnitureEntity, furnitureDefinition);
        }

        public IRoomFloorFurniture CreateFloorFurnitureFromPlayerFurniture(IRoomFurnitureManager roomFurnitureManager, IPlayerFurniture playerFurniture)
        {
            if (playerFurniture is PlayerFurniture furniture) return CreateFloorFurniture(roomFurnitureManager, furniture.FurnitureEntity);

            return null;
        }

        public IRoomWallFurniture CreateWallFurniture(IRoomFurnitureManager roomFurnitureManager, FurnitureEntity furnitureEntity)
        {
            var furnitureDefinition = _furnitureManager.GetFurnitureDefinition(furnitureEntity.FurnitureDefinitionEntityId);

            if (furnitureDefinition == null) return null;

            return ActivatorUtilities.CreateInstance<RoomWallFurniture>(_provider, roomFurnitureManager, furnitureEntity, furnitureDefinition);
        }

        public IRoomWallFurniture CreateWallFurnitureFromPlayerFurniture(IRoomFurnitureManager roomFurnitureManager, IPlayerFurniture playerFurniture)
        {
            if (playerFurniture is PlayerFurniture furniture) return CreateWallFurniture(roomFurnitureManager, furniture.FurnitureEntity);

            return null;
        }
    }
}
