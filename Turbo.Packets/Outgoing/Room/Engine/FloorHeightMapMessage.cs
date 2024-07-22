using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record FloorHeightMapMessage : IComposer
{
    public bool IsZoomedIn { get; init; }
    public int WallHeight { get; init; }
    public IRoomModel RoomModel { get; init; }
}