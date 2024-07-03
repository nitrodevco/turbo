using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Object.Logic.Wired.Data
{
    public interface IWiredActionData : IWiredData
    {
        public int Delay { get; }
        public IList<int> Conflicts { get; }
    }
}