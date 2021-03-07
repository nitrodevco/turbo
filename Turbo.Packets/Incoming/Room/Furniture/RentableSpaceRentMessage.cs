namespace Turbo.Packets.Incoming.Room.Furniture
{
    public record RentableSpaceRentMessage : IMessageEvent
    {
        public int ObjectId { get; init; }
    }
}
