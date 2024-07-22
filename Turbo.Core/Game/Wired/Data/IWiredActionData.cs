using System.Collections.Generic;

namespace Turbo.Core.Game.Wired.Data;

public interface IWiredActionData : IWiredData
{
    public int Delay { get; }
    public IList<int> Conflicts { get; }
}