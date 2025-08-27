using System.Collections;
using System.Collections.Generic;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Messenger;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Security;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Players;

public interface IPlayer : IRoomObjectAvatarHolder, IRoomManipulator, IPermissionHolder, ISessionHolder, IComponent
{
    public IPlayerManager PlayerManager { get; }
    public IPlayerDetails PlayerDetails { get; }
    public IPlayerInventory PlayerInventory { get; }
    public IPlayerWallet PlayerWallet { get; }
    public IMessenger Messenger { get; }

    public new int Id { get; }
    public new string Name { get; }
    public IList<PlayerPerkEnum> PlayerPerks { get; }

    public bool SetInventory(IPlayerInventory playerInventory);
    public bool SetWallet(IPlayerWallet playerWallet);
    public bool SetMessenger(IMessenger messenger);
}