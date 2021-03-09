using System;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Core.Game.Rooms;

namespace Turbo.Rooms.Managers
{
    public class RoomSecurityManager : IAsyncInitialisable, IAsyncDisposable
    {
        private readonly IRoom _room;

        public RoomSecurityManager(IRoom room)
        {
            _room = room;
        }

        public async ValueTask InitAsync()
        {

        }

        public async ValueTask DisposeAsync()
        {

        }
    }
}
