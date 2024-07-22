using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms;

namespace Turbo.Events.Game.Rooms.Furniture;

public class RemoveWallFurnitureEvent : TurboEvent
{
    public IRoomManipulator Manipulator { get; init; }
    public IRoomWallFurniture Furniture { get; init; }
}