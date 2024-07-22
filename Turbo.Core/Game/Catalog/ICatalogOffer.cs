using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Game.Catalog;

public interface ICatalogOffer
{
    public ICatalogPage Page { get; }
    public IList<ICatalogProduct> Products { get; }

    public int Id { get; }
    public int PageId { get; }
    public string LocalizationId { get; }
    public int CostCredits { get; }
    public int CostCurrency { get; }
    public int? CurrencyType { get; }
    public bool CanGift { get; }
    public bool CanBundle { get; }
    public int ClubLevel { get; }
    public bool IsPet { get; }
    public string PreviewImage { get; }
    public bool Visible { get; }

    public void SetPage(ICatalogPage catalogPage);
    public void AddProduct(ICatalogProduct catalogProduct);
    public Task<ICatalogOffer> Purchase(IPlayer player, string extraParam, int quantity);
}