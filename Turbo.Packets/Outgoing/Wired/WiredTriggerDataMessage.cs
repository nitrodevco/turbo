using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Wired
{
    public record WiredTriggerDataMessage : IComposer
    {
        public IRoomObject WiredTriggerObject { get; init; }
    }
}
