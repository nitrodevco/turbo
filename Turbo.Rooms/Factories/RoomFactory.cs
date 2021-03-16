using System;
using Turbo.Core.Game.Rooms;
using Turbo.Database.Entities.Room;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Rooms.Managers;

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
            ILogger<IRoom> logger = _provider.GetService<ILogger<Room>>();
            IRoomManager roomManager = _provider.GetService<IRoomManager>();

            return new Room(roomManager, logger, roomEntity);
        }
    }
}
