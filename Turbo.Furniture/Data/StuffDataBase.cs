using System.Text.Json.Serialization;
using Turbo.Core.Game.Furniture.Data;

namespace Turbo.Furniture.Data;

public class StuffDataBase : IStuffData
{
    [JsonIgnore] public int Flags { get; set; }

    [JsonPropertyName("U_N")] public int UniqueNumber { get; set; }

    [JsonPropertyName("U_S")] public int UniqueSeries { get; set; }

    public virtual string GetLegacyString()
    {
        return "";
    }

    public virtual void SetState(string state)
    {
    }

    public int GetState()
    {
        var state = int.Parse(GetLegacyString());

        return state;
    }

    public bool IsUnique()
    {
        return UniqueSeries > 0;
    }
}