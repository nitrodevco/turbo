using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IRoomObjectLogic : IDisposable
    {
        public bool SetRoomObject(IRoomObject roomObject);
    }
}
