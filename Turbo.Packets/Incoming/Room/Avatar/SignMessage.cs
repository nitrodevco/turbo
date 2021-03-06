namespace Turbo.Packets.Incoming.Room.Avatar
{
    public record SignMessage : IMessageEvent
    {
        public int SignId { get; init; }
    }
}
