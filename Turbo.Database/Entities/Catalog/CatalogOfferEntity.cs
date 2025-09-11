using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Database.Attributes;

namespace Turbo.Database.Entities.Catalog;

[Table("catalog_offers")]
public class CatalogOfferEntity : Entity
{
    [Column("localization_id")]
    [Required]
    public string LocalizationId { get; set; }

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

    public IList<CatalogProductEntity> Products { get; set; }
}