using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Database.Attributes;

namespace Turbo.Database.Entities.Catalog;

[Table("catalog_pages")]
public class CatalogPageEntity : Entity
{
    [Column("parent_id")] public int? ParentEntityId { get; set; }

    [Column("localization")] [Required] public string Localization { get; set; }

    [Column("name")] public string? Name { get; set; }

    [Column("icon")]
    [Required]
    [DefaultValueSql("0")]
    public int Icon { get; set; }

    [Column("layout")]
    [Required]
    [DefaultValueSql("default_3x3")]
    public string Layout { get; set; }

    [Column("image_data")] public string? ImageData { get; set; }

    [Column("text_data")] public string? TextData { get; set; }

    [Column("visible")]
    [Required]
    [DefaultValueSql("1")]
    public bool? Visible { get; set; }

    [ForeignKey(nameof(ParentEntityId))] public CatalogPageEntity ParentEntity { get; set; }

    public IList<CatalogPageEntity> Children { get; set; }

    public IList<CatalogOfferEntity> Offers { get; set; }
}