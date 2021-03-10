using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Database.Entities.Players;

namespace Turbo.Players
{
    public class Player : IPlayer
    {
        private IPlayerContainer _playerContainer { get; set; }
        private ISession _session { get; set; }
        private bool _isDisposing { get; set; }

        public IPlayerDetails PlayerDetails { get; private set; }
        public IRoomObject RoomObject { get; private set; }

        public Player(IPlayerContainer playerContainer, PlayerEntity playerEntity)
        {
            _playerContainer = playerContainer;

            PlayerDetails = new PlayerDetails(this, playerEntity);
        }

        public async ValueTask InitAsync()
        {
            // load roles
            // load inventory
            // load messenger
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

            await _session.DisposeAsync();

            PlayerDetails.SaveNow();
        }

        public bool SetSession(ISession session)
        {
            if ((_session != null) && (_session != session)) return false;

            if (!session.SetPlayer(this)) return false;

            _session = session;

            return true;
        }

        public bool SetRoomObject(IRoomObject roomObject)
        {
            ClearRoomObject();

            // room object set holder this

            RoomObject = roomObject;

            // update all messenger friends

            return true;
        }

        public void ClearRoomObject()
        {
            if(RoomObject != null)
            {
                RoomObject.Dispose();

                RoomObject = null;

                // update all messenger friends
            }

            // clear pending doorbell
            // if pending room return
            // send hotel view composer
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
