using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine
{
    public record ItemUpdateMessage : IComposer
    {
        public IRoomObject Object { get; init; }
    }
}
