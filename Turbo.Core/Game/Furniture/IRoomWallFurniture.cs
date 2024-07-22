using System;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Furniture;

public interface IRoomWallFurniture : IRoomFurniture, IRoomObjectWallHolder, IDisposable
{
    public string SavedWallLocation { get; }
}