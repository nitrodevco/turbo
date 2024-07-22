using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record PresentOpenMessage : IMessageEvent
{
    public int ObjectId { get; init; }
}