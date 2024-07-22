using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Avatar;

public record PassCarryItemToPetMessage : IMessageEvent
{
    public int PetId { get; init; }
}