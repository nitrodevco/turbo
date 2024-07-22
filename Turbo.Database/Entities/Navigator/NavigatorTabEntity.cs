using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Database.Entities.Navigator;

[Table("navigator_tabs")]
[Index(nameof(SearchCode), IsUnique = true)]
public class NavigatorTabEntity : Entity
{
    [Column("search_code")] [Required] public string SearchCode { get; set; }
}