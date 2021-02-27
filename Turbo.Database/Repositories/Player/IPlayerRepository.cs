using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Entities;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Repositories.Player
{
    public interface IPlayerRepository : IRepository<PlayerEntity>
    {
    }
}
