using Turbo.Core.Game;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture.Definition;

public class FurnitureDefinition(FurnitureDefinitionEntity _entity) : IFurnitureDefinition
{
    public int Id => _entity.Id;
    public int SpriteId => _entity.SpriteId;
    public string PublicName => _entity.PublicName;
    public string ProductName => _entity.ProductName;
    public string Type => _entity.Type;
    public string Logic => _entity.Logic;
    public int TotalStates => _entity.TotalStates;
    public int X => _entity.X;
    public int Y => _entity.Y;

    public double Z
    {
        get
        {
            if (_entity.Z == 0) return DefaultSettings.MinimumStackHeight;

            return _entity.Z;
        }
    }

    public bool CanStack => (bool)_entity.CanStack;
    public bool CanWalk => (bool)_entity.CanWalk;
    public bool CanSit => (bool)_entity.CanSit;
    public bool CanLay => (bool)_entity.CanLay;
    public bool CanRecycle => (bool)_entity.CanRecycle;
    public bool CanTrade => (bool)_entity.CanTrade;
    public bool CanGroup => (bool)_entity.CanGroup;
    public bool CanSell => (bool)_entity.CanSell;
    public FurniUsagePolicy UsagePolicy => _entity.UsagePolicy;
    public string ExtraData => _entity.ExtraData;
}