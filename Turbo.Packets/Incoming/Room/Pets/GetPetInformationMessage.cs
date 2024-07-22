using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Pets;

public record GetPetInformationMessage : IMessageEvent
{
    public int PetId { get; init; }
}