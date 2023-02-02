using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Events.Game.Rooms.Furniture
{
    public class MoveWallFurnitureEvent : TurboEvent
    {
        public IRoomManipulator Manipulator { get; init; }
        public IRoomObjectWall WallObject { get; init; }
        public string Location { get; init; }
    }
}