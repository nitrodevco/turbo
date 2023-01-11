using Microsoft.Extensions.DependencyInjection;
using System;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Furniture.Factories;
using Turbo.Rooms.Managers;
using Turbo.Core.Game.Players;

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
            IFurnitureFactory furnitureFactory = _provider.GetService<IFurnitureFactory>();
            IRoomObjectFactory roomObjectFactory = _provider.GetService<IRoomObjectFactory>();
            IPlayerManager playerManager = _provider.GetService<IPlayerManager>();
            IServiceScopeFactory scopeFactory = _provider.GetService<IServiceScopeFactory>();

            return new RoomFurnitureManager(room, furnitureFactory, roomObjectFactory, playerManager, scopeFactory);
        }
    }
}
