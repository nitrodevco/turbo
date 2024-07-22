using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Session;

public record CantConnectMessage : IComposer
{
    public CantConnectReason Reason { get; init; }
    public string Parameter { get; init; }
}