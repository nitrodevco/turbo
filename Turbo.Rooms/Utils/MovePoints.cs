using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Utils
{
    public class MovePoints
    {
        public static IReadOnlyCollection<IPoint> StandardPoints = new ReadOnlyCollection<IPoint>(new List<IPoint>
        {
            new Point(0, -1),
            new Point(1, 0),
            new Point(0, 1),
            new Point(-1, 0)
        });

        public static IReadOnlyCollection<IPoint> DiagonalPoints = new ReadOnlyCollection<IPoint>(new List<IPoint>
        {
            new Point(1, -1),
            new Point(-1, 1),
            new Point(1, 1),
            new Point(-1, -1)
        });

        public static IReadOnlyCollection<IPoint> MovingPoints = new ReadOnlyCollection<IPoint>(StandardPoints.Concat(DiagonalPoints).ToList());
    }
}
