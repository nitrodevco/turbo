using Turbo.Core.Packets.Messages;
using Turbo.Core.Game.Rooms.Object.Logic.Wired.Data;

namespace Turbo.Packets.Outgoing.Wired
{
    public record WiredDataMessage : IComposer
    {
        public IWiredData WiredData { get; init; }
    }
}