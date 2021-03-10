using System;
using Turbo.Core;
using Turbo.Core.Game.Rooms.Mapping;

namespace Turbo.Rooms
{
    public interface IRoomManager : IRoomContainer, IAsyncInitialisable, IAsyncDisposable
    {
        public IRoomModel GetModel(int id);
        public IRoomModel GetModelByName(string name);
    }
}
