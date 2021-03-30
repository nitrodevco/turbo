using Microsoft.Extensions.Logging;
using System;
using Turbo.Core.Game.Players.Components;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Core.Game.Players
{
    public interface IPlayer : IRoomObjectUserHolder, IRoomManipulator, IAsyncInitialisable, IAsyncDisposable
    {
        public ILogger<IPlayer> Logger { get; }
        public new ISession Session { get; }
        public IPlayerDetails PlayerDetails { get; }
        public IPermissionComponent PermissionComponent { get; }

        public bool SetSession(ISession session);
        public new bool HasPermission(string permission);

        public new int Id { get; }
        public new string Name { get; }
    }
}
