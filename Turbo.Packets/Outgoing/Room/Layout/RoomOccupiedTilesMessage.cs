using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Room.Layout;

public record RoomOccupiedTilesMessage : IComposer
{
    public List<IRoomTile> OccupiedTiles { get; init; }
}