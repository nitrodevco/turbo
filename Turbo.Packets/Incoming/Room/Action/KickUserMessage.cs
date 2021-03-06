namespace Turbo.Packets.Incoming.Room.Action
{
    public record KickUserMessage : IMessageEvent
    {
        public int UserId { get; init; }
    }
}
