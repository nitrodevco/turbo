using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Handshake;

public record SSOTicketMessage : IMessageEvent
{
    public string SSO { get; init; }
    public int ElapsedMilliseconds { get; init; }
}