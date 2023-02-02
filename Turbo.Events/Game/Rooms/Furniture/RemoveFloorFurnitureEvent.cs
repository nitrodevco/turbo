using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms;

namespace Turbo.Events.Game.Rooms.Furniture
{
    public class RemoveFloorFurnitureEvent : TurboEvent
    {
        public IRoomManipulator Manipulator { get; init; }
        public IRoomFloorFurniture Furniture { get; init; }
    }
}