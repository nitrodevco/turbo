using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Core.Database.Entities.Furniture;

[Table("furniture_teleport_links")]
[Index(nameof(FurnitureEntityOneId), IsUnique = true)]
[Index(nameof(FurnitureEntityTwoId), IsUnique = true)]
public class FurnitureTeleportLinkEntity : Entity
{
    [Column("furniture_one_id")] public int FurnitureEntityOneId { get; set; }

    [Column("furniture_two_id")] public int FurnitureEntityTwoId { get; set; }

    [ForeignKey(nameof(FurnitureEntityOneId))]
    public FurnitureEntity FurnitureEntityOne { get; set; }

    [ForeignKey(nameof(FurnitureEntityTwoId))]
    public FurnitureEntity FurnitureEntityTwo { get; set; }
}