using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Messages
{
    public class RoomObjectAvatarWalkPathMessage : RoomObjectUpdateMessage
    {
        public IPoint Goal { get; private set; }
        public IList<IPoint> Path { get; private set; }

        public RoomObjectAvatarWalkPathMessage(IPoint goal, IList<IPoint> path) : base(null)
        {
            Goal = goal;
            Path = path;
        }
    }
}
