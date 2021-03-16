using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turbo.Database.Entities.Furniture
{
    [Table("furniture_definitions")]
    public class FurnitureDefinitionEntity : Entity
    {
        [Column("sprite_id")]
        public int SpriteId { get; set; }

        [Column("public_name")]
        public string PublicName { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("logic")]
        public string Logic { get; set; }

        [Column("total_states")]
        public int TotalStates { get; set; }

        [Column("x")]
        public int X { get; set; }

        [Column("y")]
        public int Y { get; set; }

        [Column("z")]
        public double Z { get; set; }

        [Column("can_stack")]
        public bool CanStack { get; set; }

        [Column("can_walk")]
        public bool CanWalk { get; set; }

        [Column("can_sit")]
        public bool CanSit { get; set; }

        [Column("can_lay")]
        public bool CanLay { get; set; }

        [Column("can_recycle")]
        public bool CanRecycle { get; set; }

        [Column("can_trade")]
        public bool CanTrade { get; set; }

        [Column("can_group")]
        public bool CanGroup { get; set; }

        [Column("can_sell")]
        public bool CanSell { get; set; }

        [Column("extra_data")]
        public string ExtraData { get; set; }

        public List<FurnitureEntity> Furnitures { get; set; }
    }
}
