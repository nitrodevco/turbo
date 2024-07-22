using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record SetCustomStackingHeightMessage : IMessageEvent
{
    public int FurniId { get; init; }
    public int Height { get; init; }
}