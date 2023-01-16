using Microsoft.Extensions.DependencyInjection;
using System;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Furniture.Factories;
using Turbo.Rooms.Managers;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object.Logic;

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
            var furnitureFactory = _provider.GetService<IFurnitureFactory>();
            var roomObjectFactory = _provider.GetService<IRoomObjectFactory>();
            var roomObjectLogicFactory = _provider.GetService<IRoomObjectLogicFactory>();
            var playerManager = _provider.GetService<IPlayerManager>();
            var scopeFactory = _provider.GetService<IServiceScopeFactory>();

            return new RoomFurnitureManager(room, furnitureFactory, roomObjectFactory, roomObjectLogicFactory, playerManager, scopeFactory);
        }
    }
}
