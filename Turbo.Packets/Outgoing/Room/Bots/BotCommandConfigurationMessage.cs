using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Bots;

public record BotCommandConfigurationMessage : IComposer
{
    public int BotId { get; init; }
    public int CommandId { get; init; }
    public string Data { get; init; }
}