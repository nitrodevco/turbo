using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Players;
using Turbo.Inventory.Factories;

namespace Turbo.Players
{
    public class Player : Component, IPlayer
    {
        public ILogger<IPlayer> Logger { get; private set; }
        public IPlayerManager PlayerManager { get; private set; }
        public IPlayerDetails PlayerDetails { get; private set; }
        public IPlayerInventory PlayerInventory { get; private set; }
        public IPlayerWallet PlayerWallet { get; private set; }

        public ISession Session { get; private set; }
        public IRoomObjectAvatar RoomObject { get; private set; }

        public Player(
            ILogger<IPlayer> logger,
            IPlayerManager playerManager,
            IPlayerDetails playerDetails,
            IPlayerInventoryFactory playerInventoryFactory,
            IServiceScopeFactory serviceScopeFactory)
        {
            Logger = logger;
            PlayerManager = playerManager;
            PlayerDetails = playerDetails;
            PlayerInventory = playerInventoryFactory.Create(this);
            PlayerWallet = new PlayerWallet(this, serviceScopeFactory);
        }

        protected override async Task OnInit()
        {
            PlayerDetails.PlayerStatus = PlayerStatusEnum.Online;

            await PlayerWallet.InitAsync();
            await PlayerInventory.InitAsync();
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
        }

        public bool SetSession(ISession session)
        {
            if ((Session != null) && (Session != session)) return false;

            if (!session.SetPlayer(this)) return false;

            Session = session;

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

            if ((avatarObject == null) || !avatarObject.SetHolder(this)) return false;

            RoomObject = avatarObject;

            // update all messenger friends

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

                // update all messenger friends
            }

            PlayerManager.ClearPlayerRoomStatus(this);
        }

        public bool HasPermission(string permission)
        {
            return false;
        }

        public RoomObjectHolderType Type => RoomObjectHolderType.User;

        public int Id => PlayerDetails.Id;

        public string Name => PlayerDetails.Name;

        public string Motto => PlayerDetails.Motto;

        public string Figure => PlayerDetails.Figure;

        public AvatarGender Gender => PlayerDetails.Gender;
    }
}
