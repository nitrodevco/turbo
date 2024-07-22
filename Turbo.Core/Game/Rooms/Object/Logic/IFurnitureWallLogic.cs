namespace Turbo.Core.Game.Rooms.Object.Logic;

public interface IFurnitureWallLogic : IFurnitureLogic
{
    public IRoomObjectWall RoomObject { get; }
    public bool SetRoomObject(IRoomObjectWall roomObject);
}