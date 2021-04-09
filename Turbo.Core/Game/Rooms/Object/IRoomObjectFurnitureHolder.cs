namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectFurnitureHolder : IRoomObjectHolder
    {
        public Task<bool> SetupRoomObject();
        public string LogicType { get; }
        public string PlayerName { get; set; }
        public int PlayerId { get; }
    }
}
