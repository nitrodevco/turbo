using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Players;

namespace Turbo.Players.Factories
{
    public interface IPlayerFactory
    {
        public IPlayer Create(IPlayerContainer playerContainer, PlayerEntity playerEntity);
    }
}
