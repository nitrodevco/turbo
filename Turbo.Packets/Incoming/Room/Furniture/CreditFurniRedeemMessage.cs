namespace Turbo.Packets.Incoming.Room.Furniture
{
    public record CreditFurniRedeemMessage : IMessageEvent
    {
        public int ObjectId { get; init; }
    }
}
