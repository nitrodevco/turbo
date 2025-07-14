using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Core.Database.Entities.Navigator;

[Table("navigator_top_level_contexts")]
[Index(nameof(SearchCode), IsUnique = true)]
public class NavigatorTopLevelContextEntity : Entity
{
    [Column("search_code")][Required] public string SearchCode { get; set; }

    [Column("visible")][Required] public bool Visible { get; set; }

    [Column("order_num")][Required] public int OrderNum { get; set; }
}