using System;
using System.Collections.Generic;

namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IRoomObjectLogicFactory
    {
        public IDictionary<string, Type> Logics { get; }
        public IRoomObjectLogic Create(string type);
    }
}
