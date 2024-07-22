using System;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Rooms;
using Turbo.Database.Entities.Room;

namespace Turbo.Rooms.Factories;

public class RoomFactory(IServiceProvider _provider) : IRoomFactory
{
    public IRoom Create(RoomEntity roomEntity)
    {
        return ActivatorUtilities.CreateInstance<Room>(_provider, roomEntity);
    }
}