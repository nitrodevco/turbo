using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Rooms.Managers;

namespace Turbo.Rooms.Factories
{
    public class RoomUserFactory : IRoomUserFactory
    {
        private readonly IServiceProvider _provider;

        public RoomUserFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IRoomUserManager Create(IRoom room)
        {
            IRoomObjectFactory roomObjectFactory = _provider.GetService<IRoomObjectFactory>();

            return new RoomUserManager(room, roomObjectFactory);
        }
    }
}
