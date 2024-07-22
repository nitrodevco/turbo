using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record HeightMapMessage : IComposer
{
    public IRoomMap RoomMap { get; init; }
    public IRoomModel RoomModel { get; init; }
}