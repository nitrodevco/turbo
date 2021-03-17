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

            IRoomSecurityManager securityManager = _provider.GetService<IRoomSecurityManager>();
            IRoomFurnitureManager furnitureManager = _provider.GetService<IRoomFurnitureManager>();
            IRoomUserManager userManager = _provider.GetService<IRoomUserManager>();

            IRoomDetailsFactory detailsFactory = _provider.GetService<IRoomDetailsFactory>();
            var roomDetails = detailsFactory.Create(roomEntity);

            var room = new Room(roomManager, logger, securityManager, furnitureManager, userManager, roomDetails);

            securityManager.Room = room;
            furnitureManager.Room = room;
            userManager.Room = room;

            return room;
        }
    }
}
