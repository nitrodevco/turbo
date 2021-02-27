using System;
using System.Collections.Generic;
using System.Text;
using Turbo.Database.Entities;

namespace Turbo.Players
{
    public class PlayerDetails
    {
        private IPlayer _player { get; set; }
        private PlayerEntity _playerEntity { get; set; }

        public PlayerDetails(IPlayer player, PlayerEntity playerEntity)
        {
            _player = player;
            _playerEntity = playerEntity;
        }

        public void SaveNow()
        {

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
                return _playerEntity.Motto;
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
