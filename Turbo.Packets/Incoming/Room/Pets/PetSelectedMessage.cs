using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Pets;

public record PetSelectedMessage : IMessageEvent
{
    public int PetId { get; init; }
}