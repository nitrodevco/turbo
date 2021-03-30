using System;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Storage;

namespace Turbo.Core.Game.Furniture
{
    public interface IFurnitureManager : IAsyncInitialisable, IAsyncDisposable
    {
        public IFurnitureDefinition GetFurnitureDefinition(int id);
        public IStorageQueue StorageQueue { get; }
    }
}
