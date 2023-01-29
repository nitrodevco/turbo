using System;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Security;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Players
{
    public interface IPlayer : IRoomObjectAvatarHolder, IRoomManipulator, IPermissionHolder, ISessionHolder, IComponent
    {
        public ILogger<IPlayer> Logger { get; }
        public IPlayerManager PlayerManager { get; }
        public IPlayerDetails PlayerDetails { get; }
        public IPlayerInventory PlayerInventory { get; }

        public new int Id { get; }
        public new string Name { get; }
    }
}
