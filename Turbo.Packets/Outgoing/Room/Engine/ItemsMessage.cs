using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

/// <summary>
///     ItemsMessage is for wallitems
/// </summary>
public record ItemsMessage : IComposer
{
    public IList<IRoomObjectWall> Objects { get; init; }
    public IDictionary<int, string> OwnersIdToUsername { get; init; }
}