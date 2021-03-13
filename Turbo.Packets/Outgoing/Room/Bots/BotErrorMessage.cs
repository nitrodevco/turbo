using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record BotErrorCode : IComposer
    {
        public int ErrorCode { get; init; }
    }
}
