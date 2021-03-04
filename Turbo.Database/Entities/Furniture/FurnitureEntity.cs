using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Database.Entities.Furniture
{
    [Table("furniture")]
    public class FurnitureEntity : Entity
    {
        [Column("definition_id")]
        public FurnitureDefinitionEntity FurnitureDefinitionEntity { get; set; }
    }
}
