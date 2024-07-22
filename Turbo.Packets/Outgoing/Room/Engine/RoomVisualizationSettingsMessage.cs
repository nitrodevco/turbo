using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record RoomVisualizationSettingsMessage : IComposer
{
    public bool WallsHidden { get; init; }
    public int WallThickness { get; init; }
    public int FloorThickness { get; init; }
}