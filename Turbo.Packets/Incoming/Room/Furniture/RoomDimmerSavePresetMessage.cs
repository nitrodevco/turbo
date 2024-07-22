using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record RoomDimmerSavePresetMessage : IMessageEvent
{
    public int PresetNumber { get; init; }
    public int EffectTypeId { get; init; }
    public string ColorRgbHex { get; init; }
    public int ColorBrightness { get; init; }
    public bool SetAsSelectedPreset { get; init; }
}