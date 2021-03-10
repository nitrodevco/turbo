using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine
{
    public record FloorHeightMapMessage : IComposer
    {
        public IRoomMap RoomMap { get; init; }
    }
}
