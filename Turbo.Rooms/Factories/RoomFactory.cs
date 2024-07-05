using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Turbo.Core.Events;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Database.Entities.Room;
using Turbo.Database.Repositories.Room;
using Turbo.Rooms.Managers;

namespace Turbo.Rooms.Factories
{
    public class RoomFactory(IServiceProvider provider) : IRoomFactory
    {
        private readonly IServiceProvider _provider = provider;

        public IRoom Create(RoomEntity roomEntity)
        {
            return ActivatorUtilities.CreateInstance<Room>(_provider, roomEntity);
        }
    }
}
