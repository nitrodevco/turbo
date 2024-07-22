using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record ObjectAddMessage : IComposer
{
    public IRoomObjectFloor Object { get; init; }
    public string OwnerName { get; init; }
}