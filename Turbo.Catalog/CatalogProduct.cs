using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Catalog;
using Turbo.Core.Game.Catalog.Constants;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Players;
using Turbo.Database.Entities.Catalog;

namespace Turbo.Catalog;

public class CatalogProduct(
    ILogger<ICatalogProduct> _logger,
    CatalogProductEntity _entity
) : ICatalogProduct
{
    public ICatalogOffer Offer { get; private set; }
    public IFurnitureDefinition FurnitureDefinition { get; private set; }

    public void SetOffer(ICatalogOffer catalogOffer)
    {
        if (catalogOffer == null || Offer == catalogOffer) return;

        Offer = catalogOffer;

        catalogOffer.AddProduct(this);
    }

    public void SetFurnitureDefinition(IFurnitureDefinition furnitureDefinition)
    {
        if (furnitureDefinition == null || furnitureDefinition.Id != _entity.FurnitureDefinitionEntityId) return;

        FurnitureDefinition = furnitureDefinition;
    }

    public bool CanPlayerRecieveProduct(IPlayer player)
    {
        if (player == null) return false;

        // does player have badge
        // does player have effect

        return true;
    }

    public async Task GiveProductToPlayer(IPlayer player)
    {
        if (player == null) return;

        if ((ProductType.Equals(ProductTypeEnum.Floor) || ProductType.Equals(ProductTypeEnum.Wall)) &&
            FurnitureDefinitionId != -1)
        {
            await player.PlayerInventory.FurnitureInventory.GiveFurniture(FurnitureDefinitionId);
        }

        else if (ProductType.Equals(ProductTypeEnum.Badge))
        {
            //player.PlayerInventory?.BadgeInventory?.
            // send purchase error if recipient already has badge
        }
    }

    public int Id => _entity.Id;
    public int OfferId => _entity.CatalogOfferEntityId;
    public int FurnitureDefinitionId => _entity.FurnitureDefinitionEntityId ?? -1;
    public string ProductType => _entity.ProductType;
    public int SpriteId => FurnitureDefinition?.SpriteId ?? -1;
    public string ExtraParam => _entity.ExtraParam;
    public int Quantity => _entity.Quantity;
    public int UniqueSize => _entity.UniqueSize;
    public int UniqueRemaining => _entity.UniqueRemaining;
}