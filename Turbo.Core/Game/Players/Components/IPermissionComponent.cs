using System;
using Turbo.Core.Security.Permissions;

namespace Turbo.Core.Game.Players.Components
{
    public interface IPermissionComponent : IAsyncInitialisable, IAsyncDisposable
    {
        public IRank Rank { get; }
    }
}
