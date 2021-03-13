using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record CustomUserNotificationMessage : IComposer
    {
        public int Code { get; init; }
    }
}
