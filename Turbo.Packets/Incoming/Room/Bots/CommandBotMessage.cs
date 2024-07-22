using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Bots;

public record CommandBotMessage : IMessageEvent
{
    public int BotId { get; init; }
    public int SkillId { get; init; }
    public string Command { get; init; }
}