using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Constants;

namespace Turbo.Rooms.Object.Logic.Furniture.Games.BattleBanzai
{
    public class FurnitureBattleBanzaiTileLogic : FurnitureFloorLogic
    {
        public override FurniUsagePolicy UsagePolicy => FurniUsagePolicy.Nobody;
    }
}