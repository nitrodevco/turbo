using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record MoveObjectMessage : IMessageEvent
{
    public int ObjectId { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
    public int Direction { get; init; }
}