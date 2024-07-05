using System;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Events;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Database.Repositories.Furniture;
using Turbo.Database.Repositories.Player;
using Turbo.Furniture.Factories;
using Turbo.Rooms.Managers;

namespace Turbo.Rooms.Factories
{
    public class RoomFurnitureFactory(IServiceProvider provider) : IRoomFurnitureFactory
    {
        private readonly IServiceProvider _provider = provider;

        public IRoomFurnitureManager Create(IRoom room)
        {
            return ActivatorUtilities.CreateInstance<RoomFurnitureManager>(_provider, room);
        }
    }
}
