using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Events.Game.Rooms.Avatar;

public class AvatarLeaveFloorFurnitureEvent : TurboEvent
{
    public IRoomObjectAvatar AvatarObject { get; init; }
    public IRoomObjectFloor FloorObject { get; init; }
}