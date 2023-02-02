using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Games
{
    public interface IRoomGameTile
    {
        public IRoomGame Game { get; }
        public IRoomObjectFloor FloorObject { get; }

        public IPoint Location => FloorObject.Location;
    }
}