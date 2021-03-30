using Turbo.Core.Security.Permissions;
using Turbo.Database.Entities.Security;

namespace Turbo.Security
{
    public class Permission : IPermission
    {
        private readonly PermissionEntity _permissionEntity;

        public Permission(PermissionEntity permissionEntity)
        {
            _permissionEntity = permissionEntity;
        }

        public string Name => _permissionEntity.Name;

        public int Id => _permissionEntity.Id;
    }
}
