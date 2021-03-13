using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake
{
    public record BotCommandConfigurationMessage : IComposer
    {
        public int BotId { get; init; }
        public int CommandId { get; init; }
        public string Data { get; init; }
    }
}
