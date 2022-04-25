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
        public IRoomTile GetValidTile(IRoomObject roomObject, IPoint point, bool isGoal = true, bool blockingDisabled = false);
        public IRoomTile GetValidDiagonalTile(IRoomObject roomObject, IPoint point, bool blockingDisabled = false);
        public IPoint GetValidPillowPoint(IRoomObject userObject, IRoomObject furnitureObject, IPoint originalPoint);
        public IRoomTile GetHighestTileForRoomObject(IRoomObject roomObject);
        public void AddRoomObjects(params IRoomObject[] roomObjects);
        public void MoveRoomObject(IRoomObject roomObject, IPoint oldLocation, bool sendUpdate = true);
        public void RemoveRoomObjects(IRoomManipulator roomManipulator, params IRoomObject[] roomObjects);
        public void UpdatePoints(bool updateUsers = true, params IPoint[] points);
        public bool BlockingDisabled { get; }
    }
}
