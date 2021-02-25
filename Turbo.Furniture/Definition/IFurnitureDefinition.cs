using System;
using System.Collections.Generic;
using System.Text;

namespace Turbo.Furniture.Definition
{
    public interface IFurnitureDefinition
    {
        public int Id { get; }
        public int SpriteId { get; }
        public string PublicName { get; }
        public string ProductName { get; }
        public string Type { get; }
        public string LogicType { get; }
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
    }
}
