using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Navigator;

namespace Turbo.Navigator
{
    public class PendingRoomInfo : IPendingRoomInfo
    {
        public int RoomId { get; set; }
        public bool Approved { get; set; }
    }
}
