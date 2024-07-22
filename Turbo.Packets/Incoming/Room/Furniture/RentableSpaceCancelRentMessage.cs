using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record RentableSpaceCancelRentMessage : IMessageEvent
{
    public int ObjectId { get; init; }
}