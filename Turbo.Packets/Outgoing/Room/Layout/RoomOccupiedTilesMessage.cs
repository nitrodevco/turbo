using System.Collections.Generic;

namespace Turbo.Packets.Outgoing.Room.Layout
{
    public record RoomOccupiedTilesMessage : IComposer
    {
        public List<ITile> OccupiedTiles { get; init; }
    }

    public interface ITile
    {
        int X { get; set; }
        int Y { get; set; }
    }
}
