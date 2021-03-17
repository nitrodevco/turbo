using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Mapping
{
    public interface IRoomMap : IDisposable
    {
        public IList<IRoomTile> Tiles { get; }
        public IPathFinder PathFinder { get; }
        public void GenerateMap();
        public IRoomTile GetTile(IPoint point);
        public IRoomTile GetValidTile(IRoomObject roomObject, IPoint point, bool isGoal = true);
        public IRoomTile GetValidDiagonalTile(IRoomObject roomObject, IPoint point);
    }
}
