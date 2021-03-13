using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record UserTypingMessage : IComposer
    {
        public int UserId { get; init; }
        public bool IsTyping { get; init; }
    }
}
