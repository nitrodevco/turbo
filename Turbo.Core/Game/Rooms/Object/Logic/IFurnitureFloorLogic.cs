using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Mapping;

namespace Turbo.Core.Game.Rooms.Object.Logic;

public interface IFurnitureFloorLogic : IFurnitureLogic, IRollingObjectLogic
{
    public IRoomObjectFloor RoomObject { get; }
    public double StackHeight { get; }
    public double Height { get; }
    public bool SetRoomObject(IRoomObjectFloor roomObject);

    public void OnEnter(IRoomObjectAvatar avatar);
    public void OnLeave(IRoomObjectAvatar avatar);
    public void OnStep(IRoomObjectAvatar avatar);
    public void OnStop(IRoomObjectAvatar avatar);
    public bool CanStack();
    public bool CanWalk(IRoomObjectAvatar avatar = null);
    public bool CanSit(IRoomObjectAvatar avatar = null);
    public bool CanLay(IRoomObjectAvatar avatar = null);
    public bool CanRoll();
    public bool IsOpen(IRoomObjectAvatar avatar = null);
    public IRoomTile GetCurrentTile();
    public IList<IRoomTile> GetCurrentTiles();
}