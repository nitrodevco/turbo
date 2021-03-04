using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core;

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
