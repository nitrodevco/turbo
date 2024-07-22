using Turbo.Core.Game.Rooms.Games.Teams;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Games.Teams;

public class RoomGamePlayer : IRoomGamePlayer
{
    public RoomGamePlayer(
        IRoomGameTeam team,
        IRoomObjectAvatar avatar
    )
    {
        Team = team;
        Avatar = avatar;
    }

    public int Score { get; private set; }
    public IRoomGameTeam Team { get; }
    public IRoomObjectAvatar Avatar { get; }

    public void Reset()
    {
        Score = 0;
    }

    public void AdjustScore(int amount)
    {
        Score += amount;
    }
}