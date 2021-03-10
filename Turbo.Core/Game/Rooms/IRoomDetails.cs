namespace Turbo.Core.Game.Rooms
{
    public interface IRoomDetails
    {
        public int Id { get; }
        public string Name { get; }
        public int ModelId { get; }
        public bool AllowWalkThrough { get; }
    }
}
