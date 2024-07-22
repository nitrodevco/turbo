using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record ObjectsDataUpdateMessage : IComposer
{
    public IList<IRoomObjectFloor> Objects { get; init; }
}