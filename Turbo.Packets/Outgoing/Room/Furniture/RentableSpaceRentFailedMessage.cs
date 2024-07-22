using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record RentableSpaceRentFailedMessage : IComposer
{
    public int Reason { get; init; }
}