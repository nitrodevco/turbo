using System.Collections.Generic;

namespace Turbo.Furniture.Data.Types;

public class NumberStuffData : StuffDataBase
{
    private static readonly int _state = 0;

    public IList<int> Data { get; }

    public override string GetLegacyString()
    {
        return GetValue(_state).ToString();
    }

    public override void SetState(string state)
    {
        Data[_state] = int.Parse(state);
    }

    public int GetValue(int index)
    {
        return Data[index];
    }
}