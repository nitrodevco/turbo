using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record OpenPetPackageResultMessage : IComposer
{
    public int ObjectId { get; init; }
    public int NameValidationStatus { get; init; }
    public string NameValidationInfo { get; init; }
}