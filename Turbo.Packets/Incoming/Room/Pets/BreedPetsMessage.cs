using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Pets;

public record BreedPetsMessage : IMessageEvent
{
    public PetBreedingState State { get; init; }
    public int PetOneId { get; init; }
    public int PetTwoId { get; init; }
}