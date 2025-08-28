using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Dtos;
using Turbo.Database.Context;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Database.Repositories.Furniture;

public class FurnitureRepository(IEmulatorContext _context) : IFurnitureRepository
{
    public async Task<FurnitureEntity> FindAsync(int id)
    {
        return await _context.Furnitures
            .FirstOrDefaultAsync(furniture => furniture.Id == id);
    }

    public async Task<List<FurnitureEntity>> FindAllByRoomIdAsync(int roomId)
    {
        return await _context.Furnitures
            .Where(entity => entity.RoomEntityId == roomId)
            .ToListAsync();
    }

    public async Task<List<FurnitureEntity>> FindAllInventoryByPlayerIdAsync(int playerId)
    {
        return await _context.Furnitures
            .Where(entity => entity.PlayerEntityId == playerId && entity.RoomEntityId == null)
            .ToListAsync();
    }

    public async Task<TeleportPairingDto> GetTeleportPairingAsync(int furnitureId)
    {
        var linkEntity = await FindTeleportLinkByFurnitureIdAsync(furnitureId);

        if (linkEntity is null) return null;

        FurnitureEntity furnitureEntity;

        if (linkEntity.FurnitureEntityOneId == furnitureId)
            furnitureEntity = await FindAsync(linkEntity.FurnitureEntityTwoId);
        else
            furnitureEntity = await FindAsync(linkEntity.FurnitureEntityOneId);

        if (furnitureEntity is null) return null;

        return new TeleportPairingDto
        {
            TeleportId = furnitureEntity.Id,
            RoomId = furnitureEntity.RoomEntityId
        };
    }

    private async Task<FurnitureTeleportLinkEntity> FindTeleportLinkByFurnitureIdAsync(int furnitureId)
    {
        return await _context.FurnitureTeleportLinks
            .Where(entity => entity.FurnitureEntityOneId == furnitureId || entity.FurnitureEntityTwoId == furnitureId)
            .SingleOrDefaultAsync();
    }

    public async Task<List<MoodLightPresetDto>> GetMoodLightPresets(int furnitureId)
    {
        // Load existing presets for this item
        var presetEntities = await _context.FurnitureMoodLightPresets
            .Where(item => item.ItemEntityId == furnitureId)
            .ToListAsync();

        // If none exist, create a default set (typically 3 presets)
        if (presetEntities is null || presetEntities.Count == 0)
        {
            // Create 3 default presets
            for (var i = 0; i < 3; i++)
            {
                var entity = new FurnitureMoodLightPresetEntity
                {
                    ItemEntityId = furnitureId,
                    ColorHex = "#000000",
                    Brightness = 255,
                    EffectType = 1
                };

                _context.Add(entity);
            }

            await _context.SaveChangesAsync();

            // Reload after insert to include generated IDs
            presetEntities = await _context.FurnitureMoodLightPresets
                .Where(item => item.ItemEntityId == furnitureId)
                .ToListAsync();
        }

        var presets = new List<MoodLightPresetDto>();

        if (presetEntities is null || presetEntities.Count == 0)
            return presets;

        var presetIndex = 1;

        foreach (var presetEntity in presetEntities)
        {
            var presetDto = new MoodLightPresetDto
            {
                Id = presetEntity.Id,
                PresetId = presetIndex,
                ItemId = presetEntity.ItemEntityId,
                EffectType = presetEntity.EffectType,
                ColorHex = presetEntity.ColorHex,
                Brightness = presetEntity.Brightness
            };

            presets.Add(presetDto);
            presetIndex++;
        }

        return presets;
    }

    public async Task UpdateMoodLightPreset(int itemId, MoodLightPresetDto moodLightPresetDto)
    {
        if (moodLightPresetDto is null) return;

        var query = _context.FurnitureMoodLightPresets
            .AsTracking()
            .Where(p => p.ItemEntityId == itemId);

        FurnitureMoodLightPresetEntity entity = null;

        if (moodLightPresetDto.Id > 0)
        {
            entity = await query.FirstOrDefaultAsync(p => p.Id == moodLightPresetDto.Id);
        }
        else if (moodLightPresetDto.PresetId > 0)
        {
            var ordered = await query.OrderBy(p => p.Id).ToListAsync();
            var index = moodLightPresetDto.PresetId - 1;
            if (index >= 0 && index < ordered.Count)
                entity = ordered[index];
        }

        if (entity is null) return;

        entity.EffectType = moodLightPresetDto.EffectType;
        entity.ColorHex = moodLightPresetDto.ColorHex;
        entity.Brightness = moodLightPresetDto.Brightness;

        if (entity.ItemEntityId != itemId)
            entity.ItemEntityId = itemId;

        await _context.SaveChangesAsync();
    }
}