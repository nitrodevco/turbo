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
        public IPlayerManager PlayerManager { get; }
        public IPlayerDetails PlayerDetails { get; }
        public IPlayerInventory PlayerInventory { get; }
        public IPlayerWallet PlayerWallet { get; }
        public IPlayerSettings PlayerSettings { get; }

        public bool SetInventory(IPlayerInventory playerInventory);
        public bool SetWallet(IPlayerWallet playerWallet);
        public bool SetSettings(IPlayerSettings playerSettings);

        public new int Id { get; }
        public new string Name { get; }
    }
}
