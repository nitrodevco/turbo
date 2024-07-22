using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Handshake;

public record GenericErrorMessage : IComposer
{
    public RoomGenericErrorType ErrorCode { get; init; }
}