using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record PlacePetMessage : IMessageEvent
{
    public int PetId { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
}