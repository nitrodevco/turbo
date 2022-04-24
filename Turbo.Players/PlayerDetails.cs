using System;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Database.Entities.Players;

namespace Turbo.Players
{
    public class PlayerDetails : IPlayerDetails
    {
        private readonly IPlayer _player;
        private readonly PlayerEntity _playerEntity;

        public PlayerDetails(IPlayer player, PlayerEntity playerEntity)
        {
            _player = player;
            _playerEntity = playerEntity;
        }

        public void Save()
        {
            _player.PlayerManager.StorageQueue.Add(_playerEntity);
        }

        public int Id
        {
            get
            {
                return _playerEntity.Id;
            }
        }

        public string Name
        {
            get
            {
                return _playerEntity.Name;
            }
        }

        public string Motto
        {
            get
            {
                return _playerEntity.Motto == null ? "" : _playerEntity.Motto;
            }
        }

        public string Figure
        {
            get
            {
                return _playerEntity.Figure;
            }
        }

        public AvatarGender Gender
        {
            get
            {
                return _playerEntity.Gender;
            }
        }

        public DateTime DateCreated
        {
            get
            {
                return _playerEntity.DateCreated;
            }
        }

        public DateTime DateUpdated
        {
            get
            {
                return _playerEntity.DateUpdated;
            }
        }
    }
}
