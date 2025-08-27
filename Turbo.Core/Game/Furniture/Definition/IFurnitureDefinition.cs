using System.Collections;
using System.Collections.Generic;
using Turbo.Core.Game.Furniture.Constants;

namespace Turbo.Core.Game.Furniture.Definition;

public interface IFurnitureDefinition
{
    int Id { get; }
    int SpriteId { get; }
    string PublicName { get; }
    string ProductName { get; }
    public string Type { get; }
    string Logic { get; }
    int TotalStates { get; }
    int X { get; }
    int Y { get; }
    double Z { get; }
    IDictionary<int, double> MultiHeights { get; }
    bool CanStack { get; }
    bool CanWalk { get; }
    bool CanSit { get; }
    bool CanLay { get; }
    bool CanRecycle { get; }
    bool CanTrade { get; }
    bool CanGroup { get; }
    bool CanSell { get; }
    FurniUsagePolicy UsagePolicy { get; }
    string ExtraData { get; }
}