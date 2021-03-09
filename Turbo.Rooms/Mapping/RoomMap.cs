using System.Collections.Generic;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Mapping;

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
        }
    }
}
