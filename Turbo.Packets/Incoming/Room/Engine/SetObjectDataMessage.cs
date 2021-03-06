using System.Collections.Generic;

namespace Turbo.Packets.Incoming.Room.Engine
{
    public record SetObjectDataMessage : IMessageEvent
    {
        public int FurniId { get; init; }
        public Dictionary<string, string> Data { get; init; }
    }
}
