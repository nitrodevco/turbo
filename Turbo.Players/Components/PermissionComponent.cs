using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Components;
using Turbo.Core.Security.Constants;
using Turbo.Core.Security.Permissions;
using Turbo.Database.Context;
using Turbo.Database.Entities.Players;

namespace Turbo.Players.Components
{
    public class PermissionComponent : IPermissionComponent
    {
        private IPlayer _player;
        private readonly IPermissionManager _permissionManager;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly PlayerEntity _playerEntity;

        public PermissionComponent(IPermissionManager permissionManager,
            IServiceScopeFactory scopeFactory,
            PlayerEntity playerEntity)
        {
            _permissionManager = permissionManager;
            _scopeFactory = scopeFactory;
            _playerEntity = playerEntity;
        }

        public IRank Rank
        {
            get
            {
                return _permissionManager.Ranks[_player.PlayerDetails.Rank];
            }
        }

        public IPlayer Player
        {
            set => _player = value;
        }

        public bool HasPermission(string name, PermissionRequiredRights currentRights = PermissionRequiredRights.NONE)
        {
            if (_permissionManager.PermissionsByName.TryGetValue(name, out var perm))
            {
                return (Rank.HasPermission(name, currentRights) 
                    || _playerEntity.PlayerPermissions.Exists(p => p.PermissionEntityId == perm.Id && p.RequiredRights <= currentRights));
            }
            return false;
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask InitAsync()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IEmulatorContext>();
                context.Attach(_playerEntity);
                context.Entry(_playerEntity)
                    .Collection(p => p.PlayerPermissions)
                    .Load();
            }

            return ValueTask.CompletedTask;
        }
    }
}
