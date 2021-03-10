using Turbo.Core.Game.Rooms.Mapping.Constants;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Mapping
{
    public interface IPathFinderNode
    {
        public IPoint Location { get; }
        public IPathFinderNode NextNode { get; set; }

        public int Cost { get; set; }
        public PathFinderNodeState State { get; set; }
    }
}
