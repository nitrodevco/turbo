namespace Turbo.Packets.Outgoing.Room.Engine
{
    public record FavouriteMembershipUpdateMessage : IComposer
    {
        public int RoomIndex { get; init; }
        public int GroupId { get; init; }
        public int Status { get; init; }
        public string GroupName { get; init; }
    }
}
