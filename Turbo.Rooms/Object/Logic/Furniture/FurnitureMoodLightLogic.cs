using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Rooms.Object.Attributes;

namespace Turbo.Rooms.Object.Logic.Furniture;

[RoomObjectLogic("mood_light")]
public class FurnitureMoodLightLogic : FurnitureWallLogic
{
    private readonly Dictionary<int, MoodLightPresetDto> _presets = new();
    private MoodLightPresetDto _currentPreset;
    private bool _isEnabled;

    public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Controller;

    public List<MoodLightPresetDto> Presets => _presets.Values.ToList();
    public MoodLightPresetDto CurrentPreset => _currentPreset;
    public bool IsEnabled => _isEnabled;

    public override async Task<bool> Setup(IFurnitureDefinition furnitureDefinition, string jsonString = null)
    {
        if (!await base.Setup(furnitureDefinition, jsonString)) return false;

        if (_presets.Count == 0)
        {
            if (RoomObject.RoomObjectHolder is IRoomWallFurniture furniture)
                await LoadPresetsAsync(furniture);
        }

        return true;
    }

    public override bool SetState(int state, bool refresh = true)
    {
        if (StuffData is null) return false;

        var desiredState = BuildLegacyStateString(state);
        if (string.IsNullOrEmpty(desiredState)) return false;

        var currentState = StuffData.GetLegacyString() ?? string.Empty;

        _isEnabled = state == 2;

        if (currentState == desiredState) return false;

        StuffData.SetState(desiredState);

        if (RoomObject.RoomObjectHolder is IRoomWallFurniture wallFurniture)
            wallFurniture.Save();

        if (refresh) RefreshStuffData();

        return true;
    }

    private async Task LoadPresetsAsync(IRoomWallFurniture furniture)
    {
        _presets.Clear();

        var presetsList = await furniture.GetMoodLightPresets(furniture.Id);

        if (presetsList is not null && presetsList.Count > 0)
        {
            foreach (var preset in presetsList)
            {
                if (!_presets.ContainsKey(preset.PresetId))
                    _presets.Add(preset.PresetId, preset);
            }
        }

        var legacyStuffData = StuffData?.GetLegacyString();

        if (string.IsNullOrWhiteSpace(legacyStuffData))
        {
            _isEnabled = false;
            _currentPreset = _presets.Values.FirstOrDefault();

            if (_currentPreset is null) return;

            SetState(1, false);
            return;
        }

        var parts = legacyStuffData.Split(',');

        if (parts.Length >= 2)
        {
            _isEnabled = parts[0] == "2";

            if (int.TryParse(parts[1], out var enabledPreset))
                _currentPreset = _presets.Values.FirstOrDefault(preset => preset.PresetId == enabledPreset)
                                  ?? _presets.Values.FirstOrDefault();
            else
                _currentPreset = _presets.Values.FirstOrDefault();
        }
        else
        {
            _isEnabled = false;
            _currentPreset = _presets.Values.FirstOrDefault();
        }
    }

    public async Task SavePresetAsync(int presetId, int effectType, string colorHex, int brightness, bool apply)
    {
        var existing = _presets.Values.FirstOrDefault(preset => preset.PresetId == presetId);

        if (existing is null) return;

        var requiresUpdate = false;

        if (existing.EffectType != effectType)
        {
            existing.EffectType = effectType;
            requiresUpdate = true;
        }

        if (!string.Equals(existing.ColorHex, colorHex))
        {
            existing.ColorHex = colorHex;
            requiresUpdate = true;
        }

        if (existing.Brightness != brightness)
        {
            existing.Brightness = brightness;
            requiresUpdate = true;
        }

        if (requiresUpdate)
        {
            if (RoomObject.RoomObjectHolder is IRoomWallFurniture wallFurniture)
                await wallFurniture.UpdateMoodLightPreset(RoomObject.Id, existing);
        }

        if (apply)
        {
            _currentPreset = existing;
            SetState(IsEnabled ? 2 : 1);
        }
    }

    private string BuildLegacyStateString(int state)
    {
        var preset = _currentPreset ?? _presets.Values.FirstOrDefault();
        if (preset is null) return null;

        return $"{state},{preset.PresetId},{preset.EffectType},{preset.ColorHex},{preset.Brightness},false";
    }
}
