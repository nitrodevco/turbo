using Microsoft.Extensions.DependencyInjection;
using System;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Rooms.Managers;

namespace Turbo.Rooms.Factories
{
    public class RoomSecurityFactory : IRoomSecurityFactory
    {
        private readonly IServiceProvider _provider;

        public RoomSecurityFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IRoomSecurityManager Create(IRoom room)
        {
            IServiceScopeFactory scopeFactory = _provider.GetService<IServiceScopeFactory>();

            return new RoomSecurityManager(room, scopeFactory);
        }
    }
}
