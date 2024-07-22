using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record PlacePostItMessage : IMessageEvent
{
    public int ItemId { get; init; }
    public string Location { get; init; }
}