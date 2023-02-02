using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Games.Constants;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Rooms.Games.Teams
{
    public interface IRoomGameTeam
    {
        public IRoomGame Game { get; }
        public GameTeamColor Color { get; }
        public List<IRoomGamePlayer> Players { get; }

        public void ResetTeam();
        public IRoomGamePlayer GetPlayerForAvatar(IRoomObjectAvatar avatar);
    }
}