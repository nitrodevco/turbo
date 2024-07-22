using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Events.Game.Rooms.Furniture;

public class MoveWallFurnitureEvent : TurboEvent
{
    public IRoomManipulator Manipulator { get; init; }
    public IRoomObjectWall WallObject { get; init; }
    public string Location { get; init; }
}