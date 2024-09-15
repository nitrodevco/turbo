using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Database.Attributes;

namespace Turbo.Core.Database.Entities.Catalog;

[Table("catalog_offers")]
public class CatalogOfferEntity : Entity
{
    [Column("page_id")] [Required] public int CatalogPageEntityId { get; set; }

    [Column("localization_id")] [Required] public string LocalizationId { get; set; }

    [Column("cost_credits")]
    [Required]
    [DefaultValueSql("0")]
    public int CostCredits { get; set; }

    [Column("cost_currency")]
    [Required]
    [DefaultValueSql(0)]
    public int CostCurrency { get; set; }

    [Column("currency_type")] public int? CurrencyType { get; set; }

    [Column("can_gift")]
    [Required]
    [DefaultValueSql(true)]
    public bool? CanGift { get; set; }

    [Column("can_bundle")]
    [Required]
    [DefaultValueSql(true)]
    public bool? CanBundle { get; set; }

    [Column("club_level")]
    [Required]
    [DefaultValueSql(0)]
    public int ClubLevel { get; set; }

    [Column("visible")]
    [Required]
    [DefaultValueSql(true)]
    public bool? Visible { get; set; }

    [ForeignKey(nameof(CatalogPageEntityId))]
    public CatalogPageEntity Page { get; set; }

    public IList<CatalogProductEntity> Products { get; set; }
}