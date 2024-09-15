using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.PathFinder;

public interface IPathFinder
{
    public IList<IPoint> MakePath(IRoomObjectAvatar avatar, IPoint location);

    public bool IsValidStep(IRoomObjectAvatar avatar, IPoint location, IPoint nextLocation, IPoint goalLocation,
        bool blockingDisabled = false);
}