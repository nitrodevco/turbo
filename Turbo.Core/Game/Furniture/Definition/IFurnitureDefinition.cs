using Turbo.Core.Game.Furniture.Constants;

namespace Turbo.Core.Game.Furniture.Definition;

public interface IFurnitureDefinition
{
    public int Id { get; }
    public int SpriteId { get; }
    public string PublicName { get; }
    public string ProductName { get; }
    public string Type { get; }
    public string Logic { get; }
    public int TotalStates { get; }
    public int X { get; }
    public int Y { get; }
    public double Z { get; }
    public bool CanStack { get; }
    public bool CanWalk { get; }
    public bool CanSit { get; }
    public bool CanLay { get; }
    public bool CanRecycle { get; }
    public bool CanTrade { get; }
    public bool CanGroup { get; }
    public bool CanSell { get; }
    public FurniUsagePolicy UsagePolicy { get; }
    public string ExtraData { get; }
}