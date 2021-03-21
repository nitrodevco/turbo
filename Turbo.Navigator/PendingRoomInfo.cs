using Turbo.Core.Game.Navigator;

namespace Turbo.Navigator
{
    public class PendingRoomInfo : IPendingRoomInfo
    {
        public int RoomId { get; set; }
        public bool Approved { get; set; }
    }
}
