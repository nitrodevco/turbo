using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;

namespace Turbo.Rooms.Factories
{
    public interface IRoomUserFactory
    {
        public IRoomUserManager Create(IRoom room);
    }
}
