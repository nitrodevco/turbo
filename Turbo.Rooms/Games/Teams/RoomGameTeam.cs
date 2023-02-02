using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms.Games;
using Turbo.Core.Game.Rooms.Games.Constants;
using Turbo.Core.Game.Rooms.Games.Teams;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Games.Teams
{
    public class RoomGameTeam : IRoomGameTeam
    {
        public IRoomGame Game { get; private set; }
        public GameTeamColor Color { get; private set; }

        public List<IRoomGamePlayer> Players { get; private set; }

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

        public void ResetTeam()
        {

        }

        public IRoomGamePlayer GetPlayerForAvatar(IRoomObjectAvatar avatar)
        {
            return Players.Find(player => player.Avatar == avatar);
        }
    }
}