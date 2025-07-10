using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record ClickFurniMessage : IMessageEvent
{
    public int ObjectId { get; init; }
};
