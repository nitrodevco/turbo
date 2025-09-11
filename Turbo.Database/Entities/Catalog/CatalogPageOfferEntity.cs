using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Attributes;

namespace Turbo.Database.Entities.Catalog;

[Table("catalog_page_offers")]
[Index(nameof(CatalogPageEntityId), nameof(CatalogOfferEntityId), IsUnique = true)]
public class CatalogPageOfferEntity : Entity
{
    [Column("page_id")]
    public int CatalogPageEntityId { get; set; }

    [Column("offer_id")]
    public int CatalogOfferEntityId { get; set; }

    [Column("order_index")]
    public int? OrderIndex { get; set; }

    [Column("cost_credits")]
    [Required]
    [DefaultValueSql("0")]
    public int CostCredits { get; set; }

    [Column("cost_currency")]
    [Required]
    [DefaultValueSql(0)]
    public int CostCurrency { get; set; }

    [Column("currency_type")]
    public int? CurrencyType { get; set; }

    [Column("cost_silver")]
    [Required]
    [DefaultValueSql("0")]
    public int CostSilver { get; set; }

    [Column("visible")]
    [Required]
    [DefaultValueSql(true)]
    public bool? Visible { get; set; }

    [ForeignKey(nameof(CatalogPageEntityId))]
    public CatalogPageEntity Page { get; set; }

    [ForeignKey(nameof(CatalogOfferEntityId))]
    public CatalogOfferEntity Offer { get; set; }
}
