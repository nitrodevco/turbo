namespace Turbo.Packets.Incoming.Room.Action
{
    public record BanUserWithDurationMessage : IMessageEvent
    {
        public int UserId { get; init; }
        public int RoomId { get; init; }
        public string BanType { get; init; }
    }
}
