using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
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
            IFurnitureFactory furnitureFactory = _provider.GetService<IFurnitureFactory>();
            IRoomObjectFactory roomObjectFactory = _provider.GetService<IRoomObjectFactory>();
            IServiceScopeFactory scopeFactory = _provider.GetService<IServiceScopeFactory>();

            return new RoomFurnitureManager(room, furnitureFactory, roomObjectFactory, scopeFactory);
        }
    }
}
