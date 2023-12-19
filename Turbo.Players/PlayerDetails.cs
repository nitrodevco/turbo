using System;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Database.Entities.Players;

namespace Turbo.Players
{
    public class PlayerDetails(PlayerEntity _playerEntity) : IPlayerDetails
    {
        public int Id => _playerEntity.Id;

        public string Name => _playerEntity.Name;

        public string Motto
        {
            get => _playerEntity.Motto == null ? "" : _playerEntity.Motto;
            set
            {
                _playerEntity.Motto = value;
            }
        }

        public string Figure
        {
            get => _playerEntity.Figure;
            set
            {
                _playerEntity.Figure = value;
            }
        }

        public AvatarGender Gender
        {
            get => _playerEntity.Gender;
            set
            {
                _playerEntity.Gender = value;
            }
        }

        public PlayerStatusEnum PlayerStatus
        {
            get => _playerEntity.PlayerStatus;
            set
            {
                _playerEntity.PlayerStatus = value;
            }
        }

        public DateTime DateCreated => _playerEntity.DateCreated;

        public DateTime DateUpdated => _playerEntity.DateUpdated;
    }
}
