﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Cycles
{
    public interface IRoomCycle : IDisposable
    {
        public void Run();
    }
}
