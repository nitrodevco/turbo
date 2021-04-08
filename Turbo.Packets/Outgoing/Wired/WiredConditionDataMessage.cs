using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Wired
{
    public record WiredConditionDataMessage : IComposer
    {
        public IRoomObject WiredConditionObject { get; init; }
    }
}
