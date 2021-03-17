using System;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Core.Game.Rooms;

namespace Turbo.Rooms.Managers
{
    public class RoomSecurityManager : IRoomSecurityManager
    {
        private IRoom _room;

        public IRoom Room { set => _room = value; }

        public RoomSecurityManager()
        {
   
        }

        public async ValueTask InitAsync()
        {

        }

        public async ValueTask DisposeAsync()
        {

        }
    }
}
