using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Furniture;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture.Factories
{
    public interface IPlayerFurnitureFactory
    {
        public IPlayerFurniture Create(IPlayerFurnitureContainer playerFurnitureContainer, FurnitureEntity furnitureEntity);
    }
}