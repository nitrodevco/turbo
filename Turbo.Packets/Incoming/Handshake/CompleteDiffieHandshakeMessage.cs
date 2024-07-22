using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Handshake;

public record CompleteDiffieHandshakeMessage : IMessageEvent
{
    public string SharedKey { get; init; }
}