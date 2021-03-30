using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture.Definition
{
    public class FurnitureDefinition : IFurnitureDefinition
    {
        private readonly FurnitureDefinitionEntity _entity;

        public FurnitureDefinition(FurnitureDefinitionEntity entity)
        {
            _entity = entity;
        }

        public int Id => _entity.Id;
        public int SpriteId => _entity.SpriteId;
        public string PublicName => _entity.PublicName;
        public string ProductName => _entity.ProductName;
        public string Type => _entity.Type;
        public string Logic => _entity.Logic;
        public int TotalStates => _entity.TotalStates;
        public int X => _entity.X;
        public int Y => _entity.Y;
        public double Z => _entity.Z;
        public bool CanStack => _entity.CanStack;
        public bool CanWalk => _entity.CanWalk;
        public bool CanSit => _entity.CanSit;
        public bool CanLay => _entity.CanLay;
        public bool CanRecycle => _entity.CanRecycle;
        public bool CanTrade => _entity.CanTrade;
        public bool CanGroup => _entity.CanGroup;
        public bool CanSell => _entity.CanSell;
        public FurniUsagePolicy UsagePolicy => _entity.UsagePolicy;
        public string ExtraData => _entity.ExtraData;
    }
}
