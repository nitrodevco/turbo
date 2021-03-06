namespace Turbo.Packets.Incoming.Room.Avatar
{
    public record CustomizeAvatarWithFurniMessage : IMessageEvent
    {
        public int ObjectId { get; init; }
    }
}
