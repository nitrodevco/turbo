using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Mapping
{
    public interface IPathFinder
    {
        public IList<IPoint> MakePath(IRoomObject roomObject, IPoint location);
        public bool IsValidStep(IRoomObject roomObject, IPoint location, IPoint nextLocation, IPoint goalLocation);
    }
}
