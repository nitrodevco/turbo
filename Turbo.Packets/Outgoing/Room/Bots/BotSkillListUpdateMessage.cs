using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Bots
{
    public record BotSkillListUpdateMessage : IComposer
    {
        public int BotId { get; init; }
        public int SkillList { get; init; }
    }
}
