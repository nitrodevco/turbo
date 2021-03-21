using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Utils
{
    public class MovePoints
    {
        public static IList<IPoint> StandardPoints = new List<IPoint>(new IPoint[] { 
            new Point(-1, 0),
            new Point(0, -1),
            new Point(1, 0),
            new Point(0, 1) 
        });

        public static IList<IPoint> DiagonalPoints = new List<IPoint>(new IPoint[] {
            new Point(-1, -1),
            new Point(-1, 1),
            new Point(1, -1),
            new Point(1, 1) 
        });

        public static IReadOnlyCollection<IPoint> MovingPoints = new ReadOnlyCollection<IPoint>(StandardPoints.Concat(DiagonalPoints).ToList());
    }
}
