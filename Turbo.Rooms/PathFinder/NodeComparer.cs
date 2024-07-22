using System.Collections.Generic;

namespace Turbo.Rooms.PathFinder;

public class NodeComparer : IComparer<PathFinderNode>
{
    public int Compare(PathFinderNode x, PathFinderNode y)
    {
        if (x.F > y.F) return 1;

        if (x.F < y.F) return -1;

        return 0;
    }
}