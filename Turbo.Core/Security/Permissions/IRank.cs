using System.Collections.Generic;
using Turbo.Core.Security.Constants;

namespace Turbo.Core.Security.Permissions
{
    public interface IRank
    {
        public int Id { get; }
        public int ClientLevel { get; }
        public string Name { get; }
        public bool HasPermission(string name, PermissionRequiredRights currentRights = PermissionRequiredRights.NONE);
    }
}
