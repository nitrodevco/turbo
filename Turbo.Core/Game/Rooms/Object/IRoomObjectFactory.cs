namespace Turbo.Core.Game.Rooms.Object;

public interface IRoomObjectFactory
{
    public IRoomObjectAvatar CreateAvatarObject(IRoom room, IRoomObjectContainer<IRoomObjectAvatar> roomObjectContainer,
        int id, string logicType = string.Empty);

    public IRoomObjectFloor CreateFloorObject(IRoom room, IRoomObjectContainer<IRoomObjectFloor> roomObjectContainer,
        int id, string logicType = string.Empty);

    public IRoomObjectWall CreateWallObject(IRoom room, IRoomObjectContainer<IRoomObjectWall> roomObjectContainer,
        int id, string logicType = string.Empty);
}