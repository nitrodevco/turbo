using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Database.Entities.Players;

namespace Turbo.Players
{
    public class Player : IPlayer
    {
        private IPlayerContainer _playerContainer { get; set; }

        public ILogger<IPlayer> Logger { get; private set; }

        public ISession Session { get; set; }
        public IPlayerDetails PlayerDetails { get; private set; }
        public IRoomObject RoomObject { get; private set; }

        private bool _isDisposing { get; set; }

        public Player(
            IPlayerContainer playerContainer,
            ILogger<IPlayer> logger,
            PlayerEntity playerEntity)
        {
            _playerContainer = playerContainer;

            Logger = logger;
            PlayerDetails = new PlayerDetails(this, playerEntity);
        }

        public async ValueTask InitAsync()
        {
            // load roles
            // load inventory
            // load messenger

            Logger.LogInformation("Player initialized");
        }

        public async ValueTask DisposeAsync()
        {
            if (_isDisposing) return;

            _isDisposing = true;

            ClearRoomObject();

            if (_playerContainer != null)
            {
                await _playerContainer.RemovePlayer(Id);
            }

            // set offline in PlayerDetails

            // dispose messenger
            // dispose inventory
            // dispose roles

            await Session.DisposeAsync();

            PlayerDetails.SaveNow();

            Logger.LogInformation("Player disposed");
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
            IRoom room = null;

            if (RoomObject != null)
            {
                room = RoomObject.Room;

                RoomObject.Dispose();

                RoomObject = null;

                if (room != null) room.RemoveObserver(Session);

                // update all messenger friends
            }

            _playerContainer.ClearPlayerRoomStatus(this);
        }

        public string Type
        {
            get
            {
                return "user";
 ;          }
        }

        public int Id
        {
            get
            {
                return PlayerDetails.Id;
            }
        }

        public string Name
        {
            get
            {
                return PlayerDetails.Name;
            }
        }

        public string Motto
        {
            get
            {
                return PlayerDetails.Motto;
            }
        }

        public string Figure
        {
            get
            {
                return PlayerDetails.Figure;
            }
        }

        public string Gender
        {
            get
            {
                return PlayerDetails.Gender;
            }
        }
    }
}
