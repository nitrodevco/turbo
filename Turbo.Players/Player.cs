using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Database.Entities.Players;
using Turbo.Core.Game.Inventory;
using Turbo.Inventory.Factories;

namespace Turbo.Players
{
    public class Player : IPlayer
    {
        public ILogger<IPlayer> Logger { get; private set; }
        public IPlayerManager PlayerManager { get; private set; }
        public IPlayerDetails PlayerDetails { get; private set; }
        public IPlayerInventory PlayerInventory { get; private set; }

        public ISession Session { get; private set; }
        public IRoomObjectAvatar RoomObject { get; private set; }

        public bool IsInitialized { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool IsDisposing { get; private set; }

        public Player(
            ILogger<IPlayer> logger,
            IPlayerManager playerManager,
            PlayerEntity playerEntity,
            IPlayerInventoryFactory playerInventoryFactory)
        {
            Logger = logger;
            PlayerManager = playerManager;
            PlayerDetails = new PlayerDetails(this, playerEntity);
            PlayerInventory = playerInventoryFactory.Create(this);
        }

        public async ValueTask InitAsync()
        {
            if (IsInitialized) return;

            // load roles
            // load messenger

            await PlayerInventory.InitAsync();

            IsInitialized = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (IsDisposing) return;

            IsDisposing = true;

            ClearRoomObject();

            if (PlayerManager != null) await PlayerManager.RemovePlayer(Id);

            // set offline in PlayerDetails

            // dispose messenger
            // dispose roles

            await PlayerInventory.DisposeAsync();

            await Session.DisposeAsync();

            IsDisposed = true;
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
