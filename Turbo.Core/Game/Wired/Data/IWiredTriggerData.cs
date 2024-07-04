using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Wired.Data
{
    public interface IWiredTriggerData : IWiredData
    {
        public IList<int> Conflicts { get; }
    }
}