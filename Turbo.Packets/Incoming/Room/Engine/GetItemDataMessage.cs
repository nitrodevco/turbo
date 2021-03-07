namespace Turbo.Packets.Incoming.Room.Engine
{
    public record GetItemDataMessage : IMessageEvent
    {
        public int ObjectId { get; init; }
    }
}
