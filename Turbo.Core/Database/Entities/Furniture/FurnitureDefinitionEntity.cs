using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Attributes;
using Turbo.Core.Game.Furniture.Constants;

namespace Turbo.Core.Database.Entities.Furniture;

[Table("furniture_definitions")]
[Index(nameof(SpriteId), nameof(Type), IsUnique = true)]
public class FurnitureDefinitionEntity : Entity
{
    [Column("sprite_id")] [Required] public int SpriteId { get; set; }

    [Column("public_name")] [Required] public string PublicName { get; set; }

    [Column("product_name")] [Required] public string ProductName { get; set; }

    [Column("type")]
    [Required]
    [DefaultValueSql("'s'")]
    public string Type { get; set; }

    [Column("logic")]
    [Required]
    [MaxLength(30)]
    [DefaultValueSql("'default'")] // RoomObjectLogicType.FurnitureDefault
    public string Logic { get; set; }

    [Column("total_states")]
    [DefaultValueSql(0)]
    public int TotalStates { get; set; }

    [Column("x")]
    [Required]
    [DefaultValueSql(0)]
    public int X { get; set; }

    [Column("y")]
    [Required]
    [DefaultValueSql(1)]
    public int Y { get; set; }

    [Column("z", TypeName = "double(10,3)")]
    [Required]
    [DefaultValueSql(0.0d)]
    public double Z { get; set; }

    [Column("can_stack")]
    [Required]
    [DefaultValueSql(true)]
    public bool? CanStack { get; set; }

    [Column("can_walk")]
    [Required]
    [DefaultValueSql(false)]
    public bool? CanWalk { get; set; }

    [Column("can_sit")]
    [Required]
    [DefaultValueSql(false)]
    public bool? CanSit { get; set; }

    [Column("can_lay")]
    [Required]
    [DefaultValueSql(false)]
    public bool? CanLay { get; set; }

    [Column("can_recycle")]
    [Required]
    [DefaultValueSql(false)]
    public bool? CanRecycle { get; set; }

    [Column("can_trade")]
    [Required]
    [DefaultValueSql(true)]
    public bool? CanTrade { get; set; }

    [Column("can_group")]
    [Required]
    [DefaultValueSql(true)]
    public bool? CanGroup { get; set; }

    [Column("can_sell")]
    [Required]
    [DefaultValueSql(true)]
    public bool? CanSell { get; set; }

    [Column("usage_policy")]
    [Required]
    [DefaultValueSql(FurniUsagePolicy.Controller)] // FurniUsagePolicy.Controller
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public FurniUsagePolicy UsagePolicy { get; set; }

    [Column("extra_data")] public string? ExtraData { get; set; }

    public List<FurnitureEntity> Furnitures { get; set; }
}