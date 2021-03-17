using System;
using Turbo.Core;
using Turbo.Core.Game.Rooms;

namespace Turbo.Rooms.Managers
{
    public interface IRoomFurnitureManager : IAsyncInitialisable, IAsyncDisposable
    {
        public IRoom Room { set; }
    }
}
