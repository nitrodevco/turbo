namespace Turbo.Furniture.Data.Types;

public class LegacyStuffData : StuffDataBase
{
    public LegacyStuffData()
    {
        if (Data == null || Data.Equals("")) Data = "0";
    }

    public string Data { get; set; }

    public override string GetLegacyString()
    {
        return Data;
    }

    public override void SetState(string state)
    {
        Data = state;
    }
}