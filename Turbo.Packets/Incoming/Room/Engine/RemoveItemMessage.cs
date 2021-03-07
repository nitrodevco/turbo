namespace Turbo.Packets.Incoming.Room.Engine
{
    public record RemoveItemMessage : IMessageEvent
    {
        public int ObjectId { get; init; }
    }
}
