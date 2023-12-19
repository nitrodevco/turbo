using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Database.Entities.Room;
using Turbo.Database.Repositories.Room;
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
            IRoomSecurityFactory roomSecurityFactory = _provider.GetService<IRoomSecurityFactory>();
            IRoomFurnitureFactory roomFurnitureFactory = _provider.GetService<IRoomFurnitureFactory>();
            IRoomWiredFactory roomWiredFactory = _provider.GetService<IRoomWiredFactory>();
            IRoomUserFactory roomUserFactory = _provider.GetService<IRoomUserFactory>();

            return new Room(logger, roomManager, roomEntity, roomSecurityFactory, roomFurnitureFactory, roomWiredFactory, roomUserFactory);
        }
    }
}
