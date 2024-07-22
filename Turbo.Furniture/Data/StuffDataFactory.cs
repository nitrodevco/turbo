using System.Text.Json;
using Turbo.Core.Game.Furniture.Data;
using Turbo.Furniture.Data.Types;

namespace Turbo.Furniture.Data;

public class StuffDataFactory
{
    public static IStuffData CreateStuffData(int flags)
    {
        IStuffData stuffData = null;

        switch (flags & 0xFF)
        {
            case (int)StuffDataKey.LegacyKey:
                stuffData = new LegacyStuffData();
                break;
            case (int)StuffDataKey.MapKey:
                stuffData = new MapStuffData();
                break;
            case (int)StuffDataKey.StringKey:
                stuffData = new StringStuffData();
                break;
            case (int)StuffDataKey.VoteKey:
                stuffData = new VoteStuffData();
                break;
            case (int)StuffDataKey.EmptyKey:
                stuffData = new EmptyStuffData();
                break;
            case (int)StuffDataKey.NumberKey:
                stuffData = new NumberStuffData();
                break;
            case (int)StuffDataKey.HighscoreKey:
                stuffData = new HighscoreStuffData();
                break;
            case (int)StuffDataKey.CrackableKey:
                stuffData = new CrackableStuffData();
                break;
        }

        if (stuffData == null) return null;

        stuffData.Flags = flags;

        return stuffData;
    }

    public static IStuffData CreateStuffDataFromJson(int flags, string jsonString)
    {
        if (jsonString == null) return CreateStuffData(flags);

        IStuffData stuffData = null;

        switch (flags & 0xFF)
        {
            case (int)StuffDataKey.LegacyKey:
                stuffData = JsonSerializer.Deserialize<LegacyStuffData>(jsonString);
                break;
            case (int)StuffDataKey.MapKey:
                stuffData = JsonSerializer.Deserialize<MapStuffData>(jsonString);
                break;
            case (int)StuffDataKey.StringKey:
                stuffData = JsonSerializer.Deserialize<StringStuffData>(jsonString);
                break;
            case (int)StuffDataKey.VoteKey:
                stuffData = JsonSerializer.Deserialize<VoteStuffData>(jsonString);
                break;
            case (int)StuffDataKey.EmptyKey:
                stuffData = JsonSerializer.Deserialize<EmptyStuffData>(jsonString);
                break;
            case (int)StuffDataKey.NumberKey:
                stuffData = JsonSerializer.Deserialize<NumberStuffData>(jsonString);
                break;
            case (int)StuffDataKey.HighscoreKey:
                stuffData = JsonSerializer.Deserialize<HighscoreStuffData>(jsonString);
                break;
            case (int)StuffDataKey.CrackableKey:
                stuffData = JsonSerializer.Deserialize<CrackableStuffData>(jsonString);
                break;
        }

        if (stuffData == null) return null;

        stuffData.Flags = flags;

        if (stuffData.IsUnique()) stuffData.Flags += (int)StuffDataFlags.UniqueSet;

        return stuffData;
    }
}