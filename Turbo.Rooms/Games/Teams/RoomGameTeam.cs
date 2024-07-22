using System.Collections.Generic;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms.Games;
using Turbo.Core.Game.Rooms.Games.Constants;
using Turbo.Core.Game.Rooms.Games.Teams;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Games.Teams;

public class RoomGameTeam : IRoomGameTeam
{
    private int _maxPlayers = DefaultSettings.MaxPlayersPerTeam;
    private int _score = 0;

    public RoomGameTeam(
        IRoomGame game,
        GameTeamColor color
    )
    {
        Game = game;
        Color = color;

        Players = new List<IRoomGamePlayer>();
    }

    public IRoomGame Game { get; }
    public GameTeamColor Color { get; }

    public List<IRoomGamePlayer> Players { get; }

    public void ResetTeam()
    {
    }

    public IRoomGamePlayer GetPlayerForAvatar(IRoomObjectAvatar avatar)
    {
        return Players.Find(player => player.Avatar == avatar);
    }
}