using System;
using Turbo.Core.Security.Constants;
using Turbo.Core.Security.Permissions;

namespace Turbo.Core.Game.Players.Components
{
    public interface IPermissionComponent : IAsyncInitialisable, IAsyncDisposable
    {
        public IRank Rank { get; }
        public IPlayer Player { set; }
        public bool HasPermission(string name, PermissionRequiredRights currentRights = PermissionRequiredRights.NONE);
    }
}
