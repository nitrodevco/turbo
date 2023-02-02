using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Games.Teams;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Games.Teams
{
    public class RoomGamePlayer : IRoomGamePlayer
    {
        public IRoomGameTeam Team { get; private set; }
        public IRoomObjectAvatar Avatar { get; private set; }

        public RoomGamePlayer(
            IRoomGameTeam team,
            IRoomObjectAvatar avatar
        )
        {
            Team = team;
            Avatar = avatar;
        }
    }
}