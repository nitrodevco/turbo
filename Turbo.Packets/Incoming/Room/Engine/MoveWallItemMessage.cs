using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record MoveWallItemMessage : IMessageEvent
{
    public int ObjectId { get; init; }
    public string Location { get; init; }
}