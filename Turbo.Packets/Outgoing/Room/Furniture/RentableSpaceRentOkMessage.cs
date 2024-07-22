using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record RentableSpaceRentOkMessage : IComposer
{
    public int ExpiryTime { get; init; }
}