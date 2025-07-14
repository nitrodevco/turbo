using System;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Database.Entities.Room;
using Turbo.Core.Database.Factories.Rooms;
using Turbo.Core.Game.Rooms;

namespace Turbo.Rooms.Factories;

public class RoomFactory(IServiceProvider _provider) : IRoomFactory
{
    public IRoom Create(RoomEntity roomEntity)
    {
        return ActivatorUtilities.CreateInstance<Room>(_provider, roomEntity);
    }
}