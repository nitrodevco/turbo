using System;
using System.Collections.Generic;

namespace Turbo.Core.Security.Permissions
{
    public interface IPermissionManager : IAsyncInitialisable, IAsyncDisposable
    {
        public IDictionary<int, IPermission> PermissionsById { get; }
        public IDictionary<string, IPermission> PermissionsByName { get; }
        public IDictionary<int, IRank> Ranks { get; }
    }
}
