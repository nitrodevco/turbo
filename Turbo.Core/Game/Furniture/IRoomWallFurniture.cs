using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Furniture;

public interface IRoomWallFurniture : IRoomFurniture, IRoomObjectWallHolder, IDisposable
{
    string SavedWallLocation { get; }
    Task<List<MoodLightPresetDto>> GetMoodLightPresets(int itemId);
    Task UpdateMoodLightPreset(int itemId, MoodLightPresetDto moodLightPresetDto);
}