using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Engine;

public record GiveSupplementToPetMessage : IMessageEvent
{
    public int PetId { get; init; }
    public int SupplementId { get; init; }
}