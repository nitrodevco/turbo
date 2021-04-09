using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;

namespace Turbo.Rooms.Factories
{
    public interface IRoomWiredFactory
    {
        public IRoomWiredManager Create(IRoom room);
    }
}
