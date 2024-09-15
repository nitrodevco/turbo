using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Attributes;

namespace Turbo.Core.Database.Entities.Navigator;

[Table("navigator_categories")]
[Index(nameof(Name), IsUnique = true)]
public class NavigatorCategoryEntity : Entity
{
    [Column("name")] [Required] public string Name { get; set; }

    [Column("localization_name")] public string? LocalizationName { get; set; }

    [Column("is_public")]
    [Required]
    [DefaultValueSql(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public bool IsPublic { get; set; }
}