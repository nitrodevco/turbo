namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectFurnitureHolder : IRoomObjectHolder
    {
        public string LogicType { get; }
        public string PlayerName { get; set; }
        public int PlayerId { get; }
    }
}
