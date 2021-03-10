using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Layout
{
    public record RoomEntryTileMessage : IComposer
    {
        public int X { get; init; }
        public int Y { get; init; }
        public int Direction { get; init; }
    }
}
