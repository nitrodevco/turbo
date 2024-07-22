using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record SetRandomStateMessage : IMessageEvent
{
    public int ObjectId { get; init; }
    public int Param { get; init; }
}