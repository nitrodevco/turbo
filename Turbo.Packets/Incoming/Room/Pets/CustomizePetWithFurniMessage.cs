using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Pets;

public record CustomizePetWithFurniMessage : IMessageEvent
{
    public int ItemId { get; init; }
    public int PetId { get; init; }
}