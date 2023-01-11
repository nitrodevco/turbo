using System;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Furniture.Data;

namespace Turbo.Core.Game.Inventory
{
    public interface IPlayerFurniture : IDisposable
    {
        public IFurnitureDefinition FurnitureDefinition { get; }
        public IStuffData StuffData { get; }

        public int Id { get; }
    }
}

