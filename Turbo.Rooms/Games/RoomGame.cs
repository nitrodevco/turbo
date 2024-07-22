using System.Collections.Generic;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Games;
using Turbo.Core.Game.Rooms.Games.Constants;
using Turbo.Core.Game.Rooms.Games.Teams;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Rooms.Games.Teams;

namespace Turbo.Rooms.Games;

public class RoomGame : IRoomGame
{
    private readonly IRoom _room;

    public RoomGame(
        IRoom room
    )
    {
        _room = room;

        Teams = new Dictionary<GameTeamColor, IRoomGameTeam>();
    }

    public IDictionary<GameTeamColor, IRoomGameTeam> Teams { get; }

    public IRoomGameTeam GetTeamByColor(GameTeamColor color)
    {
        return CreateTeam(color);
    }

    public IRoomGameTeam GetActiveTeamByColor(GameTeamColor color)
    {
        if (!Teams.ContainsKey(color)) return null;

        return Teams[color];
    }

    public IRoomGameTeam GetTeamForAvatar(IRoomObjectAvatar avatar)
    {
        if (avatar == null || Teams.Count == 0) return null;

        foreach (var team in Teams.Values)
        {
            if (team == null) continue;

            var player = team.GetPlayerForAvatar(avatar);

            if (player == null) continue;

            return team;
        }

        return null;
    }

    private IRoomGameTeam CreateTeam(GameTeamColor color)
    {
        var team = GetActiveTeamByColor(color);

        if (team != null) return team;

        team = new RoomGameTeam(this, color);

        SetGateForTeam(team);
        SetScoreboardForTeam(team);

        team.ResetTeam();

        Teams.Add(team.Color, team);

        return team;
    }

    protected virtual void SetGateForTeam(IRoomGameTeam team)
    {
    }

    protected virtual void SetScoreboardForTeam(IRoomGameTeam team)
    {
    }
}