using System;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object.Logic;
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
            IRoomObjectLogicFactory logicFactory = _provider.GetService<IRoomObjectLogicFactory>();

            return new RoomWiredManager(room, logicFactory);
        }
    }
}
