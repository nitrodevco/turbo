using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectFactory
    {
        public IRoomObject Create(IRoom room, IRoomObjectContainer roomObjectContainer, int id, string type, string logicType);
    }
}
