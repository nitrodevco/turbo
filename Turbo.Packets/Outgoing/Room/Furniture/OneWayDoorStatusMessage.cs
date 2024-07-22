using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record OneWayDoorStatus : IComposer
{
    public int Id { get; init; }
    public int Status { get; init; }
}