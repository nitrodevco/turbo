using Microsoft.Extensions.DependencyInjection;
using System;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Rooms.Managers;

namespace Turbo.Rooms.Factories
{
    public class ChatFactory(IServiceProvider provider) : IChatFactory
    {
        private readonly IServiceProvider _provider = provider ?? throw new ArgumentNullException(nameof(provider));

        public IRoomChatManager Create(IRoom room)
        {
            var scope = _provider.CreateScope();
            var chatManager = scope.ServiceProvider.GetRequiredService<IRoomChatManager>();
            if (chatManager is ChatManager manager)
            {
                manager.SetRoom(room);
            }

            return chatManager;
        }
    }
}