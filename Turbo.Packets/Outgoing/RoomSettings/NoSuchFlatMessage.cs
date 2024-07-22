using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings;

public record NoSuchFlatMessage : IComposer
{
    public int RoomId { get; init; }
}