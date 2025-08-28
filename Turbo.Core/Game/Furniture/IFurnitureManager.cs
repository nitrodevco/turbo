using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Furniture;

public interface IFurnitureManager : IComponent
{
    IFurnitureDefinition GetFurnitureDefinition(int id);
    Task<TeleportPairingDto> GetTeleportPairing(int furnitureId);
    Task<List<MoodLightPresetDto>> GetMoodLightPresets(int furnitureId);
    Task UpdateMoodLightPreset(int itemId, MoodLightPresetDto moodLightPresetDto);
}