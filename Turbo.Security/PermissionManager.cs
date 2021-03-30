using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Security.Permissions;
using Turbo.Database.Context;
using Turbo.Database.Entities.Security;

namespace Turbo.Security
{
    public class PermissionManager : IPermissionManager 
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<PermissionManager> _logger;
        public IDictionary<int, IPermission> PermissionsById { get; private set; }
        public IDictionary<string, IPermission> PermissionsByName { get; private set; }
        public IDictionary<int, IRank> Ranks { get; private set; }

        public PermissionManager(ILogger<PermissionManager> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = scopeFactory;

            PermissionsById = new Dictionary<int, IPermission>();
            PermissionsByName = new Dictionary<string, IPermission>();
            Ranks = new Dictionary<int, IRank>();
        }

        private ValueTask LoadPermissions()
        {
            List<PermissionEntity> entities;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbConext = scope.ServiceProvider.GetService<IEmulatorContext>();
                entities = dbConext.Permissions.ToList();
            }

            entities.ForEach(entity =>
            {
                var perm = new Permission(entity);
                PermissionsById.Add(entity.Id, perm);
                PermissionsByName.Add(entity.Name, perm);
            });

            _logger.LogInformation("Loaded {0} permissions", PermissionsById.Count);

            return ValueTask.CompletedTask;
        }

        private ValueTask LoadRanks()
        {
            List<RankEntity> entities;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbConext = scope.ServiceProvider.GetService<IEmulatorContext>();
                entities = dbConext.Ranks
                    .Include(rank => rank.RankPermissions)
                    .ToList();
            }

            entities.ForEach(entity =>
            {
                Ranks.Add(entity.Id, new Rank(entity, this));
            });

            _logger.LogInformation("Loaded {0} ranks", Ranks.Count);

            return ValueTask.CompletedTask;
        }

        public async ValueTask InitAsync()
        {
            await LoadPermissions();
            await LoadRanks();
        }

        public ValueTask DisposeAsync()
        {
            PermissionsById.Clear();
            Ranks.Clear();
            return ValueTask.CompletedTask;
        }
    }
}
