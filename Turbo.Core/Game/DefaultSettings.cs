using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.PathFinder.Constants;

namespace Turbo.Core.Game
{
    public class DefaultSettings
    {
        public static HeuristicFormula PathingFormula = HeuristicFormula.Manhattan;
        public static bool PathingAllowsDiagonals = true;
        public static double MaximumStepHeight = 2;
        public static double MaximumFurnitureHeight = 40;
    }
}