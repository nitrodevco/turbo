using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record SetMannequinNameMessage : IMessageEvent
{
    public int FurniId { get; init; }
    public string Name { get; init; }
}