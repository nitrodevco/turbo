namespace Turbo.Packets.Incoming.Handshake
{
    public record SSOTicketMessage : IMessageEvent
    {
        public string SSO { get; init; }
        public int ElapsedMilliseconds { get; init; }
    }
}
