using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Database.Repositories.Furniture;

public interface IFurnitureRepository : IBaseRepository<FurnitureEntity>
{
    Task<List<FurnitureEntity>> FindAllByRoomIdAsync(int roomId);
    Task<List<FurnitureEntity>> FindAllInventoryByPlayerIdAsync(int playerId);
    Task<TeleportPairingDto> GetTeleportPairingAsync(int furnitureId);
    Task<List<MoodLightPresetDto>> GetMoodLightPresets(int furnitureId);
    Task UpdateMoodLightPreset(int furnitureId, MoodLightPresetDto moodLightPresetDto);
}