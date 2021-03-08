using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Rooms.Object;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Mapping
{
    public class RoomMap : IRoomMap
    {
        private readonly IRoom _room;

        private readonly List<List<IRoomTile>> _map;
        private readonly List<IRoomTile> _tiles;

        public RoomMap(IRoom room)
        {
            _room = room;

            _map = new List<List<IRoomTile>>();
            _tiles = new List<IRoomTile>();
        }

        public void Dispose()
        {
            _map.Clear();
            _tiles.Clear();
        }

        public void GenerateMap()
        {
            IRoomModel roomModel = _room.RoomModel;

            if (roomModel == null) return;

            _map.Clear();
            _tiles.Clear();

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

                    if (_map[x] == null) _map[x] = new List<IRoomTile>();

                    _map[x][y] = roomTile;

                    _tiles.Add(roomTile);
                }
            }
        }

        public IRoomTile GetTile(IPoint point)
        {
            if ((point == null) || (_map[point.X] == null) || (_map[point.X][point.Y] == null)) return null;

            IRoomTile roomTile = _map[point.X][point.Y];

            if ((roomTile == null) || (roomTile.State == RoomTileState.CLOSED)) return null;

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
