using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turbo.Core.Database.Entities.Navigator;

[Table("navigator_flatcats")]
public class NavigatorFlatCategoryEntity : Entity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [Required]
    public string Name { get; set; }

    [Column("visible")]
    [Required]
    public bool Visible { get; set; }

    [Column("automatic")]
    [Required]
    public bool Automatic { get; set; }

    [Column("automatic_category")]
    public string AutomaticCategory { get; set; }

    [Column("global_category")]
    public string GlobalCategory { get; set; }

    [Column("staff_only")]
    [Required]
    public bool StaffOnly { get; set; }
    
    [Column("min_rank"), DefaultValue(1)]
    [Required]
    public int MinRank { get; set; }

    [Column("order_num")]
    [Required]
    public int OrderNum { get; set; }
}