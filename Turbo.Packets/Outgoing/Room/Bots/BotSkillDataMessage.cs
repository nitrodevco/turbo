using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record BotSkillDataMessage : IComposer
    {
        public int BotId { get; init; }
        public string Data { get; init; }
    }
}
