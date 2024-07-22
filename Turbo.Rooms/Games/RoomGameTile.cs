using Turbo.Core.Game.Rooms.Games;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Games;

public class RoomGameTile : IRoomGameTile
{
    public RoomGameTile(
        IRoomGame game,
        IRoomObjectFloor floorObject
    )
    {
        Game = game;
        FloorObject = floorObject;
    }

    public IRoomGame Game { get; }
    public IRoomObjectFloor FloorObject { get; }

    public IPoint Location => FloorObject.Location;
}