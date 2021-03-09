using System.Collections.Generic;

namespace Turbo.Packets.Outgoing.Room.Engine
{
    public record HeightMapMessage : IComposer
    {
        public int Width { get; init; }
        public int Area { get; init; }
        public List<short> HeightMap { get; init; }

        // i should have IRoomMap here or w/e so that it's generic enough for different revision serializers
        // but i would need to reference Turbo.Rooms, creating circular dependency
    }
}
