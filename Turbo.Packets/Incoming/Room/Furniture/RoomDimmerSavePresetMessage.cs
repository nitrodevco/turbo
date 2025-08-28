using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record RoomDimmerSavePresetMessage : IMessageEvent
{
    public int PresetId { get; init; }
    public int EffectType { get; init; }
    public string ColorHex { get; init; }
    public int Brightness { get; init; }
    public bool Apply { get; init; }
    public bool Unknown { get; init; }
    public int ObjectId { get; init; }
}