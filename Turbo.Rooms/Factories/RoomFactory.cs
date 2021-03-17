using System;
using Turbo.Core.Game.Rooms;
using Turbo.Database.Entities.Room;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Rooms.Factories
{
    public class RoomFactory : IRoomFactory
    {
        private readonly IServiceProvider _provider;

        public RoomFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IRoom Create(RoomEntity roomEntity)
        {
            IRoomManager roomManager = _provider.GetService<IRoomManager>();
            IRoomObjectFactory roomObjectFactory = _provider.GetService<IRoomObjectFactory>();
            ILogger<IRoom> logger = _provider.GetService<ILogger<Room>>();

            return new Room(roomManager, roomObjectFactory, logger, roomEntity);
        }
    }
}
