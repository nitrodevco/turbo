using System;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomFurnitureManager : IFurnitureContainer, IRoomObjectContainer, IAsyncInitialisable, IAsyncDisposable
    {
    }
}
