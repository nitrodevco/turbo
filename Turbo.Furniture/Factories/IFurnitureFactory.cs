using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture.Factories;

public interface IFurnitureFactory
{
    public IFurnitureDefinition GetFurnitureDefinition(int id);

    public IRoomFloorFurniture CreateFloorFurniture(IRoomFurnitureManager roomFurnitureManager,
        FurnitureEntity furnitureEntity);

    public IRoomFloorFurniture CreateFloorFurnitureFromPlayerFurniture(IRoomFurnitureManager roomFurnitureManager,
        IPlayerFurniture playerFurniture);

    public IRoomWallFurniture CreateWallFurniture(IRoomFurnitureManager roomFurnitureManager,
        FurnitureEntity furnitureEntity);

    public IRoomWallFurniture CreateWallFurnitureFromPlayerFurniture(IRoomFurnitureManager roomFurnitureManager,
        IPlayerFurniture playerFurniture);
}