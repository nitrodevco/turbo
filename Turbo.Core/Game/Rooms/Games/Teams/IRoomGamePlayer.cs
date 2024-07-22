using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Rooms.Games.Teams;

public interface IRoomGamePlayer
{
    public IRoomGameTeam Team { get; }
    public IRoomObjectAvatar Avatar { get; }
}