using System;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Events;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Furniture.Factories;
using Turbo.Rooms.Managers;

namespace Turbo.Rooms.Factories
{
    public class RoomFurnitureFactory : IRoomFurnitureFactory
    {
        private readonly IServiceProvider _provider;

        public RoomFurnitureFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IRoomFurnitureManager Create(IRoom room)
        {
            var turboEventManager = _provider.GetService<ITurboEventHub>();
            var furnitureFactory = _provider.GetService<IFurnitureFactory>();
            var roomObjectFactory = _provider.GetService<IRoomObjectFactory>();
            var roomObjectLogicFactory = _provider.GetService<IRoomObjectLogicFactory>();
            var playerManager = _provider.GetService<IPlayerManager>();
            var scopeFactory = _provider.GetService<IServiceScopeFactory>();

            return new RoomFurnitureManager(room, turboEventManager, furnitureFactory, roomObjectFactory, roomObjectLogicFactory, playerManager, scopeFactory);
        }
    }
}
