namespace Turbo.Core.Game.Rooms.Furniture;

public record RoomMoodlightData
{
    public int Id { get; init; }
    public bool Enabled { get; init; }
    public bool BackgroundOnly { get; init; }
    public string Color { get; init; }
    public int Intensity { get; init; }
}