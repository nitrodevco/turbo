using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Turbo.Core.Game.Rooms;
using Turbo.Database.Entities.Room;

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
            IRoomSecurityFactory roomSecurityFactory = _provider.GetService<IRoomSecurityFactory>();
            IRoomFurnitureFactory roomFurnitureFactory = _provider.GetService<IRoomFurnitureFactory>();
            IRoomUserFactory roomUserFactory = _provider.GetService<IRoomUserFactory>();
            ILogger<IRoom> logger = _provider.GetService<ILogger<Room>>();

            return new Room(roomManager, logger, roomSecurityFactory, roomFurnitureFactory, roomUserFactory, roomEntity);
        }
    }
}
