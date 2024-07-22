using System;
using Turbo.Core.Game.Furniture.Data;
using Turbo.Core.Game.Furniture.Definition;

namespace Turbo.Core.Game.Inventory;

public interface IPlayerFurniture : IDisposable
{
    public IFurnitureDefinition FurnitureDefinition { get; }
    public IStuffData StuffData { get; }

    public int Id { get; }
    public bool Disposed { get; }
}