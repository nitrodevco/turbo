using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Pets;

public record ScratchPetMessage : IMessageEvent
{
    public int PetId { get; init; }
}