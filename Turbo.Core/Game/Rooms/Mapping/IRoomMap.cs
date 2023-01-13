using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Mapping
{
    public interface IRoomMap : IDisposable
    {
        public byte[,] Map { get; }
        public IList<IRoomTile> Tiles { get; }
        public IPathFinder PathFinder { get; }
        public void GenerateMap();
        public IRoomTile GetTile(IPoint point);
        public IRoomTile GetValidTile(IRoomObjectAvatar avatarObject, IPoint point, bool isGoal = true, bool blockingDisabled = false);
        public IRoomTile GetValidDiagonalTile(IRoomObjectAvatar avatarObject, IPoint point, bool blockingDisabled = false);
        public IPoint GetValidPillowPoint(IRoomObjectAvatar avatarObject, IRoomObjectFloor floorObject, IPoint originalPoint);
        public IRoomTile GetHighestTileForRoomObject(IRoomObjectFloor floorObject);
        public void AddRoomObjects(params IRoomObject[] roomObjects);
        public void MoveFloorRoomObject(IRoomObjectFloor floorObject, IPoint oldLocation, bool sendUpdate = true);
        public void MoveWallRoomObject(IRoomObjectWall wallObject, string oldLocation, bool sendUpdate = true);
        public void RemoveRoomObjects(int pickerId, params IRoomObject[] roomObjects);
        public void UpdatePoints(bool updateAvatars = true, params IPoint[] points);
        public bool BlockingDisabled { get; }
    }
}
