using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture.Definition;

namespace Turbo.Core.Game.Catalog
{
    public interface ICatalogOffer
    {
        public ICatalogPage Page { get; }
        public IList<ICatalogProduct> Products { get; }

        public void SetPage(ICatalogPage catalogPage);
        public void AddProduct(ICatalogProduct catalogProduct);

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
    }
}