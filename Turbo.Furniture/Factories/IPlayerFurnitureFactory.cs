using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Inventory;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture.Factories;

public interface IPlayerFurnitureFactory
{
    public IPlayerFurniture Create(IPlayerFurnitureContainer playerFurnitureContainer, FurnitureEntity furnitureEntity);

    public IPlayerFurniture CreateFromRoomFurniture(IPlayerFurnitureContainer playerFurnitureContainer,
        IRoomFurniture roomFurniture, int playerId);

    public Task<IPlayerFurniture> CreateFromDefinitionId(IPlayerFurnitureContainer playerFurnitureContainer,
        int furnitureDefinitonId, int playerId);
}