using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

/// <summary>
///     ObjectsMessage is for floor items
/// </summary>
public record ObjectsMessage : IComposer
{
    public IDictionary<int, string> OwnersIdToUsername { get; init; }
    public IList<IRoomObjectFloor> Objects { get; init; }
}