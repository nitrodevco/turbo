using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Events.Game.Rooms.Avatar;

public class AvatarEnterRoomEvent : TurboEvent
{
    public IRoomObjectAvatar Avatar { get; init; }
}