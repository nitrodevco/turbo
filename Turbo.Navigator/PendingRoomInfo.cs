using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Navigator;

public class PendingRoomInfo : IPendingRoomInfo
{
    public int RoomId { get; set; }
    public bool Approved { get; set; }
    public IPoint Location { get; set; }
}