using System;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Storage;
using Turbo.Database.Entities.Players;

namespace Turbo.Players
{
    public class PlayerDetails : IPlayerDetails
    {
        private readonly PlayerEntity _playerEntity;
        private readonly IStorageQueue _storageQueue;

        public PlayerDetails(PlayerEntity playerEntity, IStorageQueue storageQueue)
        {
            _playerEntity = playerEntity;
            _storageQueue = storageQueue;
        }

        public void Save()
        {
            _storageQueue.Add(_playerEntity);
        }

        public async Task SaveNow()
        {
            await _storageQueue.SaveNow(_playerEntity);
        }

        public int Id => _playerEntity.Id;

        public string Name => _playerEntity.Name;

        public string Motto
        {
            get => _playerEntity.Motto == null ? "" : _playerEntity.Motto;
            set
            {
                _playerEntity.Motto = value;

                Save();
            }
        }

        public string Figure
        {
            get => _playerEntity.Figure;
            set
            {
                _playerEntity.Figure = value;

                Save();
            }
        }

        public AvatarGender Gender
        {
            get => _playerEntity.Gender;
            set
            {
                _playerEntity.Gender = value;

                Save();
            }
        }

        public PlayerStatusEnum PlayerStatus
        {
            get => _playerEntity.PlayerStatus;
            set
            {
                _playerEntity.PlayerStatus = value;

                Save();
            }
        }

        public DateTime DateCreated => _playerEntity.DateCreated;

        public DateTime DateUpdated => _playerEntity.DateUpdated;
    }
}
