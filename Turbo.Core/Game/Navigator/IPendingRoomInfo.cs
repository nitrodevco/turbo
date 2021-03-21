namespace Turbo.Core.Game.Navigator
{
    public interface IPendingRoomInfo
    {
        public int RoomId { get; set; }
        public bool Approved { get; set; }
    }
}
