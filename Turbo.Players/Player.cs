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
    public IPlayerPerks PlayerPerks { get; private set; }

    public ISession Session { get; private set; }
    public IRoomObjectAvatar RoomObject { get; private set; }

    public bool SetSession(ISession session)
    {
        if (Session != null && Session != session) return false;

        if (!session.SetPlayer(this)) return false;

        Session = session;

        return true;
    }

    public bool SetInventory(IPlayerInventory playerInventory)
    {
        if (PlayerInventory != null && PlayerInventory != playerInventory) return false;

        PlayerInventory = playerInventory;

        return true;
    }

    public bool SetWallet(IPlayerWallet playerWallet)
    {
        if (PlayerWallet != null && PlayerWallet != playerWallet) return false;

        PlayerWallet = playerWallet;

        return true;
    }

    public bool SetPerks(IPlayerPerks playerPerks)
    {
        if (PlayerPerks != null && PlayerPerks != playerPerks) return false;

        PlayerPerks = playerPerks;

        return true;
    }

    public async Task<bool> SetupRoomObject()
    {
        if (RoomObject == null) return false;

        return true;
    }

    public bool SetRoomObject(IRoomObjectAvatar avatarObject)
    {
        ClearRoomObject();

        if (avatarObject == null || !avatarObject.SetHolder(this)) return false;

        RoomObject = avatarObject;

        // TODO notify messenger friends that you've entered a room

        return true;
    }

    public void ClearRoomObject()
    {
        if (RoomObject != null)
        {
            var room = RoomObject.Room;

            if (room != null) room.RemoveObserver(Session);

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

    protected override async Task OnInit()
    {
        PlayerDetails.PlayerStatus = PlayerStatusEnum.Online;

        if (PlayerWallet != null) await PlayerWallet.InitAsync();
        if (PlayerInventory != null) await PlayerInventory.InitAsync();
    }

    protected override async Task OnDispose()
    {
        ClearRoomObject();

        if (PlayerManager != null) await PlayerManager.RemovePlayer(Id);

        PlayerDetails.PlayerStatus = PlayerStatusEnum.Offline;

        // dispose messenger
        // dispose roles

        await PlayerWallet.DisposeAsync();
        await PlayerInventory.DisposeAsync();
        await Session.DisposeAsync();
        await PlayerDetails.DisposeAsync();
    }
}