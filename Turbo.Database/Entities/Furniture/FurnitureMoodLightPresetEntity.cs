using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Attributes;

namespace Turbo.Database.Entities.Furniture;

[Table("furniture_moodlight_presets")]
[Index(nameof(ItemEntityId))]
public class FurnitureMoodLightPresetEntity : Entity
{
    [Column("item_id")]
    [Required]
    public int ItemEntityId { get; set; }

    [Column("color_hex")]
    [DefaultValueSql("'#000000'")]
    public string ColorHex { get; set; }
    
    [Column("brightness")]
    [DefaultValueSql(255)]
    public int Brightness { get; set; }

    [Column("effect_type")]
    [DefaultValueSql(1)]
    public int EffectType { get; set; }

    [ForeignKey(nameof(ItemEntityId))]
    public FurnitureEntity ItemEntity { get; set; }
}
