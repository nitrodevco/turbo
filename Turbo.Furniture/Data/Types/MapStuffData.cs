using System.Collections.Generic;

namespace Turbo.Furniture.Data.Types;

public class MapStuffData : StuffDataBase
{
    private static readonly string _state = "state";
    public IDictionary<string, string> Data { get; }

    public override string GetLegacyString()
    {
        if (Data.TryGetValue(_state, out var state)) return state;

        return "";
    }

    public override void SetState(string state)
    {
        Data.Remove(_state);
        Data.Add(_state, state);
    }

    public string GetValue(string key)
    {
        if (Data.TryGetValue(key, out var value)) return value;

        return "";
    }
}