using System.Threading.Tasks;
using Turbo.Database.Entities.Players;
using Turbo.Packets.Sessions;

namespace Turbo.Players
{
    public class Player : IPlayer
    {
        private IPlayerContainer _playerContainer { get; set; }
        private ISession _session { get; set; }
        private bool _isDisposing { get; set; }

        public PlayerDetails PlayerDetails { get; private set; }

        public Player(IPlayerContainer playerContainer, PlayerEntity playerEntity)
        {
            _playerContainer = playerContainer;

            PlayerDetails = new PlayerDetails(this, playerEntity);
        }

        public bool SetSession(ISession session)
        {
            if ((_session != null) && (_session != session)) return false;

            if (!session.SetSessionPlayer(this)) return false;

            _session = session;

            return true;
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

            // remove assigned RoomObject

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
    }
}
