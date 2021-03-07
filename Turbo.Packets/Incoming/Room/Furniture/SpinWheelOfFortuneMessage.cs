namespace Turbo.Packets.Incoming.Room.Furniture
{
    public record SpinWheelOfFortuneMessage : IMessageEvent
    {
        public int ObjectId { get; init; }
    }
}
