using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Core.Database.Attributes;

namespace Turbo.Core.Database.Entities.Catalog;

[Table("catalog_pages")]
public class CatalogPageEntity : Entity
{
    [Column("parent_id")] public int? ParentEntityId { get; set; }

    [Column("localization")]
    [Required]
    [MaxLength(50)]
    public string Localization { get; set; }

    [Column("name")] 
    [MaxLength(50)]
    public string? Name { get; set; }

    [Column("icon")]
    [Required]
    [DefaultValueSql(0)]
    public int Icon { get; set; }

    [Column("layout")]
    [Required]
    [DefaultValueSql("'default_3x3'")]
    [MaxLength(50)]
    public string Layout { get; set; }

    [Column("image_data")]
    public List<string> ImageData { get; set; } = null!;

    [Column("text_data")] public List<string> TextData { get; set; } = null!;

    [Column("visible")]
    [Required]
    [DefaultValueSql(true)]
    public bool? Visible { get; set; }

    [ForeignKey(nameof(ParentEntityId))] public CatalogPageEntity ParentEntity { get; set; }

    public IList<CatalogPageEntity> Children { get; set; }

    public IList<CatalogOfferEntity> Offers { get; set; }
}