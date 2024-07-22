using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Events.Game.Rooms.Avatar;

public class AvatarEnterFloorFurnitureEvent : TurboEvent
{
    public IRoomObjectAvatar AvatarObject { get; init; }
    public IRoomObjectFloor FloorObject { get; init; }
}