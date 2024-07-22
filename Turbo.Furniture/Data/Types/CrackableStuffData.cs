namespace Turbo.Furniture.Data.Types;

public class CrackableStuffData : StuffDataBase
{
    public string State { get; set; }
    public int Hits { get; set; }
    public int Target { get; set; }

    public override string GetLegacyString()
    {
        return State == null ? "" : State;
    }

    public override void SetState(string state)
    {
        State = state;
    }
}