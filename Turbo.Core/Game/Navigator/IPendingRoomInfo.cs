using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Navigator
{
    public interface IPendingRoomInfo
    {
        public int RoomId { get; set; }
        public bool Approved { get; set; }
    }
}
