using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Events.Game.Rooms.Furniture
{
    public class PlaceFloorFurnitureEvent : TurboEvent
    {
        public IPlayer Player { get; init; }
        public int FurniId { get; init; }
        public IPoint Location { get; init; }
    }
}