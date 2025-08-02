using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Furniture;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Inventory;

namespace Turbo.Core.Database.Factories.Players;

public interface IPlayerFurnitureFactory
{
    public IPlayerFurniture Create(IPlayerFurnitureContainer playerFurnitureContainer, FurnitureEntity furnitureEntity);

    public IPlayerFurniture CreateFromRoomFurniture(IPlayerFurnitureContainer playerFurnitureContainer,
        IRoomFurniture roomFurniture, int playerId);

    public Task<IPlayerFurniture> CreateFromDefinitionId(IPlayerFurnitureContainer playerFurnitureContainer,
        int furnitureDefinitonId, int playerId);
}