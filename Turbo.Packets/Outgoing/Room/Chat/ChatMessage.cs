using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record ChatMessage : IComposer
    {
        public int UserId { get; init; }
        public string Text { get; init; }
        public int Gesture { get; init; }
        public int StyleId { get; init; }
        public IList<string> Links { get; init; }
        public int AnimationLength { get; init; }
    }
}
