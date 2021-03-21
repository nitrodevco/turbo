using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;

namespace Turbo.Rooms.Managers
{
    public class RoomSecurityManager : IRoomSecurityManager
    {
        private IRoom _room;

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
