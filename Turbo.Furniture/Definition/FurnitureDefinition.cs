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

        public int Id
        {
            get
            {
                return _entity.Id;
            }
        }

        public int SpriteId
        {
            get
            {
                return _entity.SpriteId;
            }
        }

        public string PublicName
        {
            get
            {
                return _entity.PublicName;
            }
        }

        public string ProductName
        {
            get
            {
                return _entity.ProductName;
            }
        }

        public string Type
        {
            get
            {
                return _entity.Type;
            }
        }

        public string Logic
        {
            get
            {
                return _entity.Logic;
            }
        }

        public int X
        {
            get
            {
                return _entity.X;
            }
        }

        public int Y
        {
            get
            {
                return _entity.Y;
            }
        }

        public double Z
        {
            get
            {
                return _entity.Z;
            }
        }
    }
}
