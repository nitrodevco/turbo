using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record MovePetMessage : IMessageEvent
{
    public int Id { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
    public int Direction { get; init; }
}