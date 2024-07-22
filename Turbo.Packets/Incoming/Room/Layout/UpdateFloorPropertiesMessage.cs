using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Layout;

public record UpdateFloorPropertiesMessage : IMessageEvent
{
    public string Model { get; init; }
    public int DoorX { get; init; }
    public int DoorY { get; init; }
    public Rotation DoorRotation { get; init; }
    public int WallThickness { get; init; }
    public int FloorThickness { get; init; }
    public int? WallHeight { get; init; }
}