using System;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Database.Repositories.Room;
using Turbo.Rooms.Managers;

namespace Turbo.Rooms.Factories
{
    public class RoomSecurityFactory(IServiceProvider _provider) : IRoomSecurityFactory
    {
        public IRoomSecurityManager Create(IRoom room)
        {
            return ActivatorUtilities.CreateInstance<RoomSecurityManager>(_provider, room);
        }
    }
}
