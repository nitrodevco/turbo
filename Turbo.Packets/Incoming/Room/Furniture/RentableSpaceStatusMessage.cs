namespace Turbo.Packets.Incoming.Room.Furniture
{
    public record RentableSpaceStatusMessage : IMessageEvent
    {
        public int ObjectId { get; init; }
    }
}
