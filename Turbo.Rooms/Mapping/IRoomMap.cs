using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.RoomObject.Object;
using Turbo.RoomObject.Utils;

namespace Turbo.Rooms.Mapping
{
    public interface IRoomMap : IDisposable
    {
        public void GenerateMap();
        public IRoomTile GetTile(IPoint point);
        public IRoomTile GetValidTile(IRoomObject roomObject, IPoint point, bool isGoal = true);
        public IRoomTile GetValidDiagonalTile(IRoomObject roomObject, IPoint point);
    }
}
