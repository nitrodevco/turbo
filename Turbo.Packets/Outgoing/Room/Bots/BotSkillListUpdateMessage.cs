using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record BotSkillListUpdateMessage : IComposer
    {
        public int BotId { get; init; }
        public int SkillList { get; init; }
    }
}
