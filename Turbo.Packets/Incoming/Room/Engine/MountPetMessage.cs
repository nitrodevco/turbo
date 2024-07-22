using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record MountPetMessage : IMessageEvent
{
    public int PetId { get; init; }
    public bool Mounted { get; init; }
}