using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings;

public record UserUnbannedFromRoomMessage : IComposer
{
    public int RoomId { get; init; }
    public int PlayerId { get; init; }
}