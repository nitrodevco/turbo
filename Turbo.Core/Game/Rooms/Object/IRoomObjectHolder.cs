namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectHolder
    {
        public bool SetRoomObject(IRoomObject roomObject);
        public void ClearRoomObject();
        public string Type { get; }
        public int Id { get; }
        public string Name { get; }
        public string Motto { get; }
        public string Figure { get; }
        public IRoomObject RoomObject { get; }
    }
}
