using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record RoomDimmerGetPresetsMessage : IMessageEvent
{
    public int ObjectId { get; init; }
}