using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record BotForceOpenContextMenuMessage : IComposer
    {
        public int BotId { get; init; }
    }
}
