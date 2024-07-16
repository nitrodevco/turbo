using Microsoft.Extensions.DependencyInjection;
using System;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Rooms.Managers;

namespace Turbo.Rooms.Factories
{
    public class RoomChatFactory(IServiceProvider provider) : IRoomChatFactory
    {
        private readonly IServiceProvider _provider = provider ?? throw new ArgumentNullException(nameof(provider));

        public IRoomChatManager Create(IRoom room)
        {
            return ActivatorUtilities.CreateInstance<RoomChatManager>(_provider, room);
        }
    }
}