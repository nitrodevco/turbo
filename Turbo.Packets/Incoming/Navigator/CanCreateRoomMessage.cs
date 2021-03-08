namespace Turbo.Packets.Incoming.Navigator
{
    public record CanCreateRoomMessage : IMessageEvent
    {
        public int PlayerCurrentRoomsCount { get; init; }
        public int PlayerMaxRoomsCount { get; init; }
    }
}
