using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Messages
{
    public class RoomObjectAvatarDanceMessage : RoomObjectUpdateMessage
    {
        public int DanceType { get; private set; }

        public RoomObjectAvatarDanceMessage(int danceType) : base(null)
        {
            DanceType = danceType;
        }
    }
}
