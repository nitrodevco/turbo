using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Packets.Outgoing.Wired
{
    public record WiredTriggerDataMessage
    {
        public IRoomObject WiredTiggerObject { get; init; }
    }
}
