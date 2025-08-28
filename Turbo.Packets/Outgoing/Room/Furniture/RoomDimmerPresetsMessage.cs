using System.Collections.Generic;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Rooms.Furniture;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record RoomDimmerPresetsMessage : IComposer
{
    public int SelectedPresetId { get; init; }
    public List<MoodLightPresetDto> Presets { get; init; }
    public bool IsOn { get; init; }
    public int ItemId { get; init; }
}