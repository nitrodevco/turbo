using System.ComponentModel.DataAnnotations.Schema;

namespace Turbo.Database.Entities.Furniture
{
    [Table("furniture")]
    public class FurnitureEntity : Entity
    {
        [Column("definition_id")]
        public int FurnitureDefinitionEntityId { get; set; }

        public FurnitureDefinitionEntity FurnitureDefinitionEntity { get; set; }
    }
}
