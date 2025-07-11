using Turbo.Core.Game.Players.Rooms;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Players.Rooms;

//TODO Move IPendingRoomInfo to Player.Rooms 
public class PendingRoomInfo : IPendingRoomInfo
{
    public int RoomId { get; set; }
    public bool Approved { get; set; }
    public IPoint Location { get; set; }
}
