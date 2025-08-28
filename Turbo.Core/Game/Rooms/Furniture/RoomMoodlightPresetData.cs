namespace Turbo.Core.Game.Rooms.Furniture;

public record RoomMoodlightPresetData
{
    public int Id { get; init; }
    public int EffectType { get; init; }
    public string ColorHex { get; init; }
    public int Brightness { get; init; }
}