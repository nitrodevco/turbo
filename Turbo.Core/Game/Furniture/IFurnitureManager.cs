using System;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Storage;

namespace Turbo.Core.Game.Furniture
{
    public interface IFurnitureManager : IAsyncInitialisable, IAsyncDisposable
    {
        public IFurnitureDefinition GetFurnitureDefinition(int id);
        public Task<TeleportPairingDto> GetTeleportPairing(int furnitureId);
        public IStorageQueue StorageQueue { get; }
    }
}
