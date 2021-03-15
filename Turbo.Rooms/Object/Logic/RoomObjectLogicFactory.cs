using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Object.Logic;

namespace Turbo.Rooms.Object.Logic
{
    public class RoomObjectLogicFactory : IRoomObjectLogicFactory
    {
        private IDictionary<string, Type> _logics;

        public IRoomObjectLogic GetLogic(string type)
        {
            Type logicType = _logics[type];

            if (logicType == null) return null;

            return (IRoomObjectLogic) Activator.CreateInstance(logicType);
        }
    }
}
