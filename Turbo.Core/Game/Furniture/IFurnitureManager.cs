using System;
using Turbo.Core.Game.Furniture.Definition;

namespace Turbo.Core.Game.Furniture
{
    public interface IFurnitureManager : IAsyncInitialisable, IAsyncDisposable
    {
        public IFurnitureDefinition GetFurnitureDefinition(int id);
    }
}
