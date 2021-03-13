using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record ExpressionMessage : IComposer
    {
        public int UserId { get; init; }
        public int ExpressionType { get; init; }
    }
}
