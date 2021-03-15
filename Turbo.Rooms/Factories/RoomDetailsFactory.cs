using System;
using Turbo.Core.Game.Rooms;
using Turbo.Database.Entities.Room;

namespace Turbo.Rooms.Factories
{
    public class RoomDetailsFactory : IRoomDetailsFactory
    {
        private readonly IServiceProvider _provider;

        public RoomDetailsFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IRoomDetails Create(RoomEntity roomEntity)
        {
            return new RoomDetails(roomEntity);
        }
    }
}
