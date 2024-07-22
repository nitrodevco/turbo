using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Furniture;

public interface IFurnitureManager : IComponent
{
    public IFurnitureDefinition GetFurnitureDefinition(int id);
    public Task<TeleportPairingDto> GetTeleportPairing(int furnitureId);
}