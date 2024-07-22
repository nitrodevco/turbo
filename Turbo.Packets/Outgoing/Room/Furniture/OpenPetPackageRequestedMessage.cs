using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Furniture;

public record OpenPetPackageRequestedMessage : IComposer
{
    public int ObjectId { get; init; }
    //public PetFigureData {get; init; } todo: implement this
}