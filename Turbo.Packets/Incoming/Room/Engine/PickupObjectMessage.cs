using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record PickupObjectMessage : IMessageEvent
{
    public int ObjectCategory { get; init; }
    public int ObjectId { get; init; }
}