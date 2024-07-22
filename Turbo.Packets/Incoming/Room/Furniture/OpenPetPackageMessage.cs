using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record OpenPetPackageMessage : IMessageEvent
{
    public int ObjectId { get; init; }
    public string PetName { get; init; }
}