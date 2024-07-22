using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Utils;

public class MovePoints
{
    public static IReadOnlyCollection<IPoint> StandardPoints = new List<IPoint>(new IPoint[]
    {
        new Point(1),
        new Point(0, 1),
        new Point(-1),
        new Point(0, -1)
    });

    public static IReadOnlyCollection<IPoint> DiagonalPoints = new List<IPoint>(new IPoint[]
    {
        new Point(0, -1),
        new Point(1),
        new Point(0, 1),
        new Point(-1),
        new Point(1, -1),
        new Point(1, 1),
        new Point(-1, 1),
        new Point(-1, -1)
    });
}