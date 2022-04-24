using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Turbo.Core.Game.Furniture.Constants;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Attributes;

namespace Turbo.Database.Entities.Furniture
{
    [Table("furniture_definitions"), Index(nameof(SpriteId), nameof(Type), IsUnique = true)]
    public class FurnitureDefinitionEntity : Entity
    {
        [Column("sprite_id"), Required]
        public int SpriteId { get; set; }

        [Column("public_name"), Required]
        public string PublicName { get; set; }

        [Column("product_name"), Required]
        public string ProductName { get; set; }

        [Column("type"), Required, DefaultValueSql("'s'")]
        public string Type { get; set; }

        [Column("logic"), Required, DefaultValueSql("'default'")] // RoomObjectLogicType.FurnitureDefault
        public string Logic { get; set; }

        [Column("total_states"), DefaultValueSql("0")]
        public int TotalStates { get; set; }

        [Column("x"), DefaultValueSql("1")]
        public int X { get; set; }

        [Column("y"), DefaultValueSql("1")]
        public int Y { get; set; }

        [Column("z", TypeName = "double(10,3)"), DefaultValueSql("0")]
        public double Z { get; set; }

        [Column("can_stack"), DefaultValueSql("1")]
        public bool? CanStack { get; set; }

        [Column("can_walk"), DefaultValueSql("0")]
        public bool? CanWalk { get; set; }

        [Column("can_sit"), DefaultValueSql("0")]
        public bool? CanSit { get; set; }

        [Column("can_lay"), DefaultValueSql("0")]
        public bool? CanLay { get; set; }

        [Column("can_recycle"), DefaultValueSql("0")]
        public bool? CanRecycle { get; set; }

        [Column("can_trade"), DefaultValueSql("1")]
        public bool? CanTrade { get; set; }

        [Column("can_group"), DefaultValueSql("1")]
        public bool? CanGroup { get; set; }

        [Column("can_sell"), DefaultValueSql("1")]
        public bool? CanSell { get; set; }

        [Column("usage_policy"), DefaultValueSql("1")] // FurniUsagePolicy.Controller
        public FurniUsagePolicy UsagePolicy { get; set; }

        [Column("extra_data")]
        public string? ExtraData { get; set; }

        public List<FurnitureEntity> Furnitures { get; set; }
    }
}
