using Turbo.Core.Security.Constants;
using Turbo.Core.Security.Permissions;
using Turbo.Database.Entities.Security;

namespace Turbo.Security
{
    public class Rank : IRank
    {
        private readonly RankEntity _rankEntity;
        private readonly IPermissionManager _permissionManager;

        public int ClientLevel => _rankEntity.ClientLevel;

        public string Name => _rankEntity.Name;

        public int Id => _rankEntity.Id;

        public Rank(RankEntity rankEntity, IPermissionManager permissionManager)
        {
            _rankEntity = rankEntity;

            _permissionManager = permissionManager;
        }

        public bool HasPermission(string name, PermissionRequiredRights currentRights = PermissionRequiredRights.NONE)
        {
            if(_permissionManager.PermissionsByName.TryGetValue(name, out var perm))
            {
                return _rankEntity.RankPermissions.Exists(p => p.PermissionEntityId == perm.Id && p.RequiredRights <= currentRights);
            }
            return false;
        }
    }
}
