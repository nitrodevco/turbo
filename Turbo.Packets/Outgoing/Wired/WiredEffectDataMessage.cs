using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Wired
{
    public record WiredEffectDataMessage : IComposer
    {
        public IRoomObject WiredEffectObject { get; init; }
    }
}
