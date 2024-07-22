using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.RoomSettings;

public record BannedUsersFromRoomMessage : IComposer
{
    public int RoomId { get; init; }
    public IDictionary<int, string> Players { get; init; }
}