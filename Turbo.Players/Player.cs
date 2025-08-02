using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Utilities;

namespace Turbo.Players;

public class Player(
    ILogger<IPlayer> _logger,
    IPlayerManager _playerManager,
    IPlayerDetails _playerDetails) : Component, IPlayer
{
    public IPlayerManager PlayerManager { get; } = _playerManager;
    public IPlayerDetails PlayerDetails { get; } = _playerDetails;
    public IPlayerInventory PlayerInventory { get; private set; }
    public IPlayerWallet PlayerWallet { get; private set; }
    public ISession Session { get; private set; }
    public IRoomObjectAvatar RoomObject { get; private set; }

    public bool SetSession(ISession session)
    {
        if (Session is not null && Session != session) return false;

        if (!session.SetPlayer(this)) return false;

        Session = session;

        return true;
    }

    public bool SetInventory(IPlayerInventory playerInventory)
    {
        if (PlayerInventory is not null && PlayerInventory != playerInventory) return false;

        PlayerInventory = playerInventory;

        return true;
    }

    public bool SetWallet(IPlayerWallet playerWallet)
    {
        if (PlayerWallet is not null && PlayerWallet != playerWallet) return false;

        PlayerWallet = playerWallet;

        return true;
    }

    public Task<bool> SetupRoomObject()
    {
        if (RoomObject is null) return Task.FromResult(false);

        return Task.FromResult(true);
    }

    public bool SetRoomObject(IRoomObjectAvatar avatarObject)
    {
        ClearRoomObject();

        if (avatarObject is null || !avatarObject.SetHolder(this)) return false;

        RoomObject = avatarObject;

        // TODO notify messenger friends that you've entered a room

        return true;
    }

    public void ClearRoomObject()
    {
        if (RoomObject is not null)
        {
            var room = RoomObject.Room;

            room?.RemoveObserver(Session);

            RoomObject.Dispose();

            RoomObject = null;

            // TODO notify messenger friends that you've left a room
        }

        PlayerManager.ClearPlayerRoomStatus(this);
    }

    public bool HasPermission(string permission) => false;

    public RoomObjectHolderType Type => RoomObjectHolderType.User;

    public int Id => PlayerDetails.Id;

    public string Name => PlayerDetails.Name;

    public string Motto => PlayerDetails.Motto;

    public string Figure => PlayerDetails.Figure;

    public AvatarGender Gender => PlayerDetails.Gender;

    public IList<PlayerPerkEnum> PlayerPerks => PlayerDetails.PlayerPerks;

    protected override async Task OnInit()
    {
        PlayerDetails.PlayerStatus = PlayerStatusEnum.Online;

        if (PlayerWallet is not null) await PlayerWallet.InitAsync();
        if (PlayerInventory is not null) await PlayerInventory.InitAsync();
    }

    protected override async Task OnDispose()
    {
        ClearRoomObject();

        if (PlayerManager is not null) await PlayerManager.RemovePlayer(Id);

        PlayerDetails.PlayerStatus = PlayerStatusEnum.Offline;

        // dispose messenger
        // dispose roles

        if (PlayerWallet is not null) await PlayerWallet.DisposeAsync();
        if (PlayerInventory is not null) await PlayerInventory.DisposeAsync();
        if (Session is not null) await Session.DisposeAsync();
        await PlayerDetails.DisposeAsync();
    }
}