namespace Turbo.Packets.Incoming.Room.Action
{
    public record RemoveAllRightsMessage : IMessageEvent
    {
        public int RoomId { get; init; }
    }
}
