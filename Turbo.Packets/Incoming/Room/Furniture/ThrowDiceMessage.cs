namespace Turbo.Packets.Incoming.Room.Furniture
{
    public record ThrowDiceMessage : IMessageEvent
    {
        public int ObjectId { get; init; }
    }
}
