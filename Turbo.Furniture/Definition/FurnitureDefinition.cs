using System;
using System.Collections.Generic;
using System.Text;

namespace Turbo.Furniture.Definition
{
    public class FurnitureDefinition : IFurnitureDefinition
    {
        public int Id { get; private set; }
        public int SpriteId { get; private set; }
        public string PublicName { get; private set; }
        public string ProductName { get; private set; }
        public string Type { get; private set; }
        public string LogicType { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        public FurnitureDefinition()
        {

        }
    }
}
