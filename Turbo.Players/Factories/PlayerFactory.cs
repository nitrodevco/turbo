using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Components;
using Turbo.Core.Security.Permissions;
using Turbo.Database.Entities.Players;
using Turbo.Players.Components;

namespace Turbo.Players.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IServiceProvider _provider;

        public PlayerFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IPlayer Create(IPlayerContainer playerContainer, PlayerEntity playerEntity)
        {
            ILogger<IPlayer> logger = _provider.GetService<ILogger<Player>>();
            IPermissionManager permissionManager = _provider.GetService<IPermissionManager>();
            IServiceScopeFactory serviceScopeFactory = _provider.GetService<IServiceScopeFactory>();

            IPermissionComponent permissionComponent = new PermissionComponent(permissionManager, serviceScopeFactory, playerEntity);
            var player = new Player(playerContainer, logger, playerEntity, permissionComponent);
            permissionComponent.Player = player;
            
            return player;
        }
    }
}
