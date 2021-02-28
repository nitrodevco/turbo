using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Database.Entities.Furniture
{
    public class FurnitureDefinitionEntity : Entity
    {
        [Column("public_name")]
        public string PublicName { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("sprite_id")]
        public int SpriteId { get; set; }

        [Column("type")]
        public string type { get; set; }

        [Column("x")]
        public int X { get; set; }

        [Column("y")]
        public int Y { get; set; }

        [Column("z")]
        public double Z { get; set; }

        [Column("logic")]
        public string Logic { get; set; }

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
        public bool ExtraData { get; set; }
    }
}
