using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Events.Game.Rooms.Furniture;

public class MoveFloorFurnitureEvent : TurboEvent
{
    public IRoomManipulator Manipulator { get; init; }
    public IRoomObjectFloor FloorObject { get; init; }
    public IPoint Location { get; init; }
}