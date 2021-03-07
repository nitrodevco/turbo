namespace Turbo.Packets.Incoming.Room.Furniture
{
    public record GetGuildFurniContextMenuInfoMessage : IMessageEvent
    {
        public int ObjectId { get; init; }
        public int GuildId { get; init; }
    }
}
