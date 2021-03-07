using System;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Rooms.Object;

namespace Turbo.Rooms.Managers
{
    public class RoomFurnitureManager : IAsyncInitialisable, IAsyncDisposable
    {
        private readonly IRoom _room;

        private readonly IRoomObjectManager _roomObjectManager;

        public RoomFurnitureManager(IRoom room)
        {
            _room = room;

            _roomObjectManager = new RoomObjectManager();
        }

        public async ValueTask InitAsync()
        {

        }

        public async ValueTask DisposeAsync()
        {

        }
    }
}
