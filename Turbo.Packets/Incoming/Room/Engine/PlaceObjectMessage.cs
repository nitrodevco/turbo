using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record PlaceObjectMessage : IMessageEvent
{
    public int ObjectId { get; init; }
    public string? WallLocation { get; init; }
    public int? X { get; init; }
    public int? Y { get; init; }
    public int? Direction { get; init; }
}