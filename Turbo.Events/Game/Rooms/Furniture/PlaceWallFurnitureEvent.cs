using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;

namespace Turbo.Events.Game.Rooms.Furniture
{
    public class PlaceWallFurnitureEvent : TurboEvent
    {
        public IPlayer Player { get; init; }
        public int FurniId { get; init; }
        public string Location { get; init; }
    }
}