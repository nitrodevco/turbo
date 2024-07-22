using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Mapping;

public interface IRoomMap : IDisposable
{
    public byte[,] Map { get; }
    public IList<IRoomTile> Tiles { get; }
    public IPathFinder PathFinder { get; }
    public bool BlockingDisabled { get; }
    public void GenerateMap();
    public IRoomTile GetTile(IPoint point);

    public IRoomTile GetValidTile(IRoomObjectAvatar avatarObject, IPoint point, bool isGoal = true,
        bool blockingDisabled = false);

    public IRoomTile GetValidDiagonalTile(IRoomObjectAvatar avatarObject, IPoint point, bool blockingDisabled = false);

    public IPoint GetValidPillowPoint(IRoomObjectAvatar avatarObject, IRoomObjectFloor floorObject,
        IPoint originalPoint);

    public IRoomTile GetHighestTileForRoomObject(IRoomObjectFloor floorObject);
    public void AddFloorObject(IRoomObjectFloor floorObject);
    public void AddWallObject(IRoomObjectWall wallObject);
    public void AddAvatarObject(IRoomObjectAvatar avatarObject);
    public void MoveFloorRoomObject(IRoomObjectFloor floorObject, IPoint oldLocation, bool sendUpdate = true);
    public void MoveWallRoomObject(IRoomObjectWall wallObject, string oldLocation, bool sendUpdate = true);
    public void RemoveFloorObject(IRoomObjectFloor floorObject, int pickerId = -1);
    public void RemoveWallObject(IRoomObjectWall wallObject, int pickerId = -1);
    public void RemoveAvatarObject(IRoomObjectAvatar avatarObject);
    public void UpdatePoints(bool updateAvatars = true, params IPoint[] points);
}