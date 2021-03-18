using System;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Rooms.Mapping
{
    public class PathFinderNode : IPathFinderNode
    {
        public IPoint Location { get; set; }
        public IPathFinderNode NextNode { get; set; }

        public int Cost { get; set; }
        public bool IsOpen { get; set; }
        public bool IsClosed { get; set; }

        public PathFinderNode(IPoint location)
        {
            Location = location;

            Cost = Int32.MaxValue;
        }
    }
}
