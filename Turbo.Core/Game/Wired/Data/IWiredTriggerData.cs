using System.Collections.Generic;

namespace Turbo.Core.Game.Wired.Data;

public interface IWiredTriggerData : IWiredData
{
    public IList<int> Conflicts { get; }
}