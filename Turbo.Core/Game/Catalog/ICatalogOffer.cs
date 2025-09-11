using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Game.Catalog;

public interface ICatalogOffer
{
    int Id { get; }
    int PageId { get; }
    // Underlying CatalogOfferEntity Id (base offer shared across pages)
    int OfferEntityId { get; }
    string LocalizationId { get; }
    int CostCredits { get; }
    int CostCurrency { get; }
    int? CurrencyType { get; }
    int CostSilver { get; }
    int OrderIndex { get; }
    bool CanGift { get; }
    bool CanBundle { get; }
    int ClubLevel { get; }
    bool IsPet { get; }
    string PreviewImage { get; }
    bool Visible { get; }
    ICatalogPage Page { get; }
    IList<ICatalogProduct> Products { get; }
    void SetPage(ICatalogPage catalogPage);
    void AddProduct(ICatalogProduct catalogProduct);
    Task<ICatalogOffer> Purchase(IPlayer player, string extraParam, int quantity);
}