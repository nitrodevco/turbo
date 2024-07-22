using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record GuildFurniContextMenuInfoMessage : IComposer
{
    public int ObjectId { get; init; }
    public int GuildId { get; init; }
    public string GuildName { get; init; }
    public int RoomId { get; init; }
    public bool IsMember { get; init; }
    public bool HasForum { get; init; }
}