using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.PathFinder.Constants;

namespace Turbo.Core.Game
{
    public class DefaultSettings
    {
        public static int GameCycleMs = 500;
        public static int StoreCycleMs = 10000;
        public static HeuristicFormula PathingFormula = HeuristicFormula.Manhattan;
        public static bool PathingAllowsDiagonals = true;
        public static double MaximumStepHeight = 2.0;
        public static double MaximumFurnitureHeight = 40.0;
        public static int TileHeightDefault = 32767;
        public static int TileHeightMultiplier = 256;
        public static double MinimumStackHeight = 0.001;
        public static int FurniPerFragment = 100;
        public static int BadgesPerFragment = 100;
        public static int MaxActiveBadges = 5;
        public static int DiceCycles = 4;
    }
}