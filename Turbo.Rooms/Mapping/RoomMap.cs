using System.Collections.Generic;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Mapping
{
    public class RoomMap : IRoomMap
    {
        private readonly IRoom _room;

        private readonly IList<IList<IRoomTile>> _map;
        
        public IList<IRoomTile> Tiles { get; init; }
        public IPathFinder PathFinder { get; init; }

        public RoomMap(IRoom room)
        {
            _room = room;

            _map = new List<IList<IRoomTile>>();
            Tiles = new List<IRoomTile>();

            PathFinder = new PathFinder(this);
        }

        public void Dispose()
        {
            _map.Clear();
            Tiles.Clear();
        }

        public void GenerateMap()
        {
            IRoomModel roomModel = _room.RoomModel;

            if (roomModel == null) return;

            _map.Clear();
            Tiles.Clear();

            int totalX = roomModel.TotalX;
            int totalY = roomModel.TotalY;

            if ((totalX == 0) || (totalY == 0)) return;

            for(int y = 0; y < totalY; y++)
            {
                for(int x = 0; x < totalX; x++)
                {
                    int height = roomModel.GetTileHeight(x, y);
                    RoomTileState state = roomModel.GetTileState(x, y);

                    IRoomTile roomTile = new RoomTile(_room, new Point(x, y), height, state);

                    if (_map.Count - 1 < x) _map.Add(new List<IRoomTile>());
                    
                    _map[x].Add(roomTile);

                    Tiles.Add(roomTile);
                }
            }
        }

        public IRoomTile GetTile(IPoint point)
        {
            if ((point == null) || (_map[point.X] == null) || (_map[point.X][point.Y] == null)) return null;

            IRoomTile roomTile = _map[point.X][point.Y];

            if ((roomTile == null) || (roomTile.State == RoomTileState.Closed)) return null;

            return roomTile;
        }

        public IRoomTile GetValidTile(IRoomObject roomObject, IPoint point, bool isGoal = true)
        {
            if ((roomObject == null) || (point == null)) return null;

            IRoomTile roomTile = GetTile(point);

            if (roomTile == null) return null;

            if (roomTile.IsDoor) return roomTile;

            if(roomTile.Users.Count > 0)
            {
                foreach(IRoomObject tileRoomObject in roomTile.Users.Values)
                {
                    if (tileRoomObject == roomObject) return roomTile;
                }

                if (_room.RoomDetails.AllowWalkThrough)
                {
                    if (isGoal) return null;
                }
                else return null;
            }

            if (!roomTile.CanWalk()) return null;

            return roomTile;
        }

        public IRoomTile GetValidDiagonalTile(IRoomObject roomObject, IPoint point)
        {
            if ((roomObject == null) || (point == null)) return null;

            IRoomTile roomTile = GetTile(point);

            if (roomTile == null) return null;

            if (roomTile.IsDoor) return roomTile;

            if (roomTile.Users.Count > 0)
            {
                foreach (IRoomObject tileRoomObject in roomTile.Users.Values)
                {
                    if (tileRoomObject == roomObject) return roomTile;
                }

                if (!_room.RoomDetails.AllowWalkThrough) return null;
            }

            if (!roomTile.CanWalk() || roomTile.CanSit() || roomTile.CanLay()) return null;

            return roomTile;
        }
    }
}
