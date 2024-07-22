using Turbo.Core.Game.Rooms.Object.Logic;

namespace Turbo.Core.Game.Rooms.Object;

public interface IRoomObjectWall : IRoomObject
{
    public IRoomObjectWallHolder RoomObjectHolder { get; }
    public IFurnitureWallLogic Logic { get; }
    public string WallLocation { get; }

    public bool SetHolder(IRoomObjectWallHolder roomObjectHolder);
    public void SetLogic(IFurnitureWallLogic logic);
    public void SetLocation(string location);
}