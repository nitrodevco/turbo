using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Database.Entities.Players;

namespace Turbo.Players
{
    public class Player : IPlayer
    {
        public ILogger<IPlayer> Logger { get; private set; }
        public IPlayerManager PlayerManager { get; private set; }
        public IPlayerDetails PlayerDetails { get; private set; }

        public ISession Session { get; private set; }
        public IRoomObject RoomObject { get; private set; }

        public bool IsInitialized { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool IsDisposing { get; private set; }

        public Player(
            ILogger<IPlayer> logger,
            IPlayerManager playerManager,
            PlayerEntity playerEntity)
        {
            Logger = logger;
            PlayerManager = playerManager;
            PlayerDetails = new PlayerDetails(this, playerEntity);
        }

        public async ValueTask InitAsync()
        {
            if (IsInitialized) return;
            // load roles
            // load inventory
            // load messenger

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
            // dispose inventory
            // dispose roles

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

        public bool SetRoomObject(IRoomObject roomObject)
        {
            ClearRoomObject();

            if ((roomObject == null) || !roomObject.SetHolder(this)) return false;

            RoomObject = roomObject;

            // update all messenger friends

            return true;
        }

        public void ClearRoomObject()
        {
            if (RoomObject != null)
            {
                IRoom room = RoomObject.Room;

                RoomObject.Dispose();

                RoomObject = null;

                if (room != null) room.RemoveObserver(Session);

                // update all messenger friends
            }

            PlayerManager.ClearPlayerRoomStatus(this);
        }

        public bool HasPermission(string permission)
        {
            return false;
        }

        public string Type => "user";

        public int Id => PlayerDetails.Id;

        public string Name => PlayerDetails.Name;

        public string Motto => PlayerDetails.Motto;

        public string Figure => PlayerDetails.Figure;

        public AvatarGender Gender => PlayerDetails.Gender;
    }
}
