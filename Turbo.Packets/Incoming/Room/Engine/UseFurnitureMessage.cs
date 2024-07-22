using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record UseFurnitureMessage : IMessageEvent
{
    public int ObjectId { get; init; }
    public int Param { get; init; }
}