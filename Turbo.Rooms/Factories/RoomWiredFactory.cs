using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Rooms.Managers;

namespace Turbo.Rooms.Factories
{
    public class RoomWiredFactory : IRoomWiredFactory
    {
        private readonly IServiceProvider _provider;

        public RoomWiredFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IRoomWiredManager Create(IRoom room)
        {
            return new RoomWiredManager(room);
        }
    }
}
