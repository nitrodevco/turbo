using System;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Database.Repositories.Room;
using Turbo.Rooms.Managers;

namespace Turbo.Rooms.Factories
{
    public class RoomSecurityFactory(IServiceProvider provider) : IRoomSecurityFactory
    {
        private readonly IServiceProvider _provider = provider;

        public IRoomSecurityManager Create(IRoom room)
        {
            var playerManager = _provider.GetService<IPlayerManager>();
            var roomBanRepository = _provider.GetService<IRoomBanRepository>();
            var roomMuteRepository = _provider.GetService<IRoomMuteRepository>();
            var roomRightRepository = _provider.GetService<IRoomRightRepository>();

            return new RoomSecurityManager(room, playerManager, roomBanRepository, roomMuteRepository, roomRightRepository);
        }
    }
}
