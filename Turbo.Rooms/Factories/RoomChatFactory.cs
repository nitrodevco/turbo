using System;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Rooms.Managers;

namespace Turbo.Rooms.Factories;

public class RoomChatFactory(IServiceProvider _provider) : IRoomChatFactory
{
    public IRoomChatManager Create(IRoom room)
    {
        return ActivatorUtilities.CreateInstance<RoomChatManager>(_provider, room);
    }
}