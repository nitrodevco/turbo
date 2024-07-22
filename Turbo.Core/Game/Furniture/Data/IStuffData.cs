namespace Turbo.Core.Game.Furniture.Data;

public interface IStuffData
{
    public int Flags { get; set; }
    public int UniqueNumber { get; }
    public int UniqueSeries { get; }
    public string GetLegacyString();
    public void SetState(string state);
    public int GetState();
    public bool IsUnique();
}