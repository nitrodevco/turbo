using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Game.Catalog;

public interface ICatalogProduct
{
    int Id { get; }
    int OfferId { get; }
    int FurnitureDefinitionId { get; }
    string ProductType { get; }
    int SpriteId { get; }
    string ExtraParam { get; }
    int Quantity { get; }
    int UniqueSize { get; }
    int UniqueRemaining { get; }
    void SetOffer(ICatalogOffer catalogOffer);
    void SetFurnitureDefinition(IFurnitureDefinition furnitureDefinition);
    bool CanPlayerRecieveProduct(IPlayer player);
    Task GiveProductToPlayer(IPlayer player);
}