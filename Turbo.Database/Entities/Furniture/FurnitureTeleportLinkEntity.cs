using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Database.Entities.Furniture
{
    [Table("furniture_teleport_links")]
    public class FurnitureTeleportLinkEntity : Entity
    {
        [Column("furniture_one_id")]
        public int FurnitureEntityOneId { get; set; }

        public FurnitureEntity FurnitureEntityOne { get; set; }

        [Column("furniture_two_id")]
        public int FurnitureEntityTwoId { get; set; }

        public FurnitureEntity FurnitureEntityTwo { get; set; }
    }
}
