using System;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Rooms.Managers;

namespace Turbo.Rooms.Factories;

public class RoomFurnitureFactory(IServiceProvider _provider) : IRoomFurnitureFactory
{
    public IRoomFurnitureManager Create(IRoom room)
    {
        return ActivatorUtilities.CreateInstance<RoomFurnitureManager>(_provider, room);
    }
}