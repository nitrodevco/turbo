using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Mapping;

public interface IRoomTile
{
    public IPoint Location { get; }
    public double DefaultHeight { get; }

    public double Height { get; }
    public int RelativeHeight { get; }
    public RoomTileState State { get; }
    public IRoomObjectFloor HighestObject { get; }

    public bool HasStackHelper { get; }

    public List<IRoomObjectAvatar> Avatars { get; }
    public List<IRoomObjectFloor> Furniture { get; }

    public bool IsDoor { get; set; }

    public void AddRoomObject(IRoomObject roomObject);
    public void RemoveRoomObject(IRoomObject roomObject);
    public IRoomObjectFloor GetFurnitureAbove(IRoomObjectFloor floorObject);
    public IRoomObjectFloor GetFurnitureUnderneath(IRoomObjectFloor floorObject);

    public void ResetTileHeight();
    public void ResetHighestObject();
    public double GetWalkingHeight();
    public bool HasLogic(Type type);

    public bool CanWalk(IRoomObjectAvatar roomObject = null);
    public bool CanSit(IRoomObjectAvatar roomObject = null);
    public bool CanLay(IRoomObjectAvatar roomObject = null);
    public bool CanStack();
}