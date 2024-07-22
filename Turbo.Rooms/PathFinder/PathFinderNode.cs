using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.PathFinder;

public struct PathFinderNode
{
    public IPoint Location;
    public IPoint LocationParent;

    public int F;
    public int G;
    public int H;
}