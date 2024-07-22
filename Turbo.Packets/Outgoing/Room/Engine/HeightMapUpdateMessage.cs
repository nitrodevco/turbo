using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Engine;

public record HeightMapUpdateMessage : IComposer
{
    public List<IRoomTile> TilesToUpdate { get; init; }
}