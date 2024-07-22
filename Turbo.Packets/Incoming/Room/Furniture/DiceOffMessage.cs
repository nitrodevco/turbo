using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record DiceOffMessage : IMessageEvent
{
    public int ObjectId { get; init; }
}