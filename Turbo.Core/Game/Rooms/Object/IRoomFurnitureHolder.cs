namespace Turbo.Core.Game.Rooms.Object;

public interface IRoomObjectFurnitureHolder<T> : IRoomObjectHolder<T> where T : IRoomObject
{
    public string LogicType { get; }
    public string PlayerName { get; }
    public int PlayerId { get; }
}