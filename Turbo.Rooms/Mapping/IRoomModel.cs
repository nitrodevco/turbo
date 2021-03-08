using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Mapping
{
    public interface IRoomModel
    {
        public int TotalX { get; }
        public int TotalY { get; }
        public int TotalSize { get; }

        public IPoint DoorLocation { get; }
        public IPoint DoorDirection { get; }

        public bool DidGenerate { get; }

        public void Generate();
        public RoomTileState GetTileState(int x, int y);
        public int GetTileHeight(int x, int y);

        public int Id { get; }
        public string Name { get; }
    }
}
