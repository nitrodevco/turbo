using Turbo.Core.Game.Furniture;
using Turbo.Database.Entities.Furniture;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Furniture.Definition;

namespace Turbo.Furniture.Factories
{
    public interface IFurnitureFactory
    {
        public IFurnitureDefinition GetFurnitureDefinition(int id);
        public IRoomFloorFurniture CreateFloorFurniture(IRoomFurnitureManager roomFurnitureManager, FurnitureEntity furnitureEntity);
        public IRoomWallFurniture CreateWallFurniture(IRoomFurnitureManager roomFurnitureManager, FurnitureEntity furnitureEntity);
    }
}
