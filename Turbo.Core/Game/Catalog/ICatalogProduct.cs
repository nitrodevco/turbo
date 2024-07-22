using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Game.Catalog;

public interface ICatalogProduct
{
    public int Id { get; }
    public int OfferId { get; }
    public int FurnitureDefinitionId { get; }
    public string ProductType { get; }
    public int SpriteId { get; }
    public string ExtraParam { get; }
    public int Quantity { get; }
    public int UniqueSize { get; }
    public int UniqueRemaining { get; }
    public void SetOffer(ICatalogOffer catalogOffer);
    public void SetFurnitureDefinition(IFurnitureDefinition furnitureDefinition);
    public bool CanPlayerRecieveProduct(IPlayer player);
    public Task GiveProductToPlayer(IPlayer player);
}