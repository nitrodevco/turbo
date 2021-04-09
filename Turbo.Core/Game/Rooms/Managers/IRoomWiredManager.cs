﻿using Turbo.Core.Game.Rooms.Object.Logic.Wired;

namespace Turbo.Core.Game.Rooms.Managers
{
    public interface IRoomWiredManager
    {
        public bool ProcessTriggers(string type, IWiredArguments wiredArguments = null);
    }
}