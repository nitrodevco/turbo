using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Database.Repositories.Furniture;

public interface IFurnitureRepository : IBaseRepository<FurnitureEntity>
{
    public Task<List<FurnitureEntity>> FindAllByRoomIdAsync(int roomId);
    public Task<List<FurnitureEntity>> FindAllInventoryByPlayerIdAsync(int playerId);
    public Task<TeleportPairingDto> GetTeleportPairingAsync(int furnitureId);
}