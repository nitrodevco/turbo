using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Rooms.Mapping
{
    public class RoomMap : IRoomMap
    {
        private IRoom Room { get; set; }

        private List<List<IRoomTile>> Map;
        private List<IRoomTile> Tiles;


        public RoomMap(IRoom room)
        {
            Room = room;

            Map = new List<List<IRoomTile>>();
            Tiles = new List<IRoomTile>();
        }

        public void Dispose()
        {
            Map.Clear();
            Tiles.Clear();
        }

        public void GenerateMap()
        {
            IRoomModel roomModel = Room
        }
    }
}
