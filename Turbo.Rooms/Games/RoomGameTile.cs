using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms.Games;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Games
{
    public class RoomGameTile : IRoomGameTile
    {
        public IRoomGame Game { get; private set; }
        public IRoomObjectFloor FloorObject { get; private set; }

        public RoomGameTile(
            IRoomGame game,
            IRoomObjectFloor floorObject
        )
        {
            Game = game;
            FloorObject = floorObject;
        }

        public IPoint Location => FloorObject.Location;
    }
}