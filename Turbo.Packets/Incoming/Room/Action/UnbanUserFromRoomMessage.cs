namespace Turbo.Packets.Incoming.Room.Action
{
    public record UnbanUserFromRoomMessage
    {
        public int UserId { get; init; }
        public int RoomId { get; init; }
    }
}
