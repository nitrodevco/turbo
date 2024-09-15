using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.PathFinder;

public interface IPathFinderNode
{
    public IPoint Location { get; }
    public IPathFinderNode NextNode { get; set; }

    public int Cost { get; set; }
    public bool IsOpen { get; set; }
    public bool IsClosed { get; set; }
}