using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record HarvestPetMessage : IMessageEvent
{
    public int PetId { get; init; }
}