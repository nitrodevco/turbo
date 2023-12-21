using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Navigator.Constants;
using Turbo.Database.Attributes;

namespace Turbo.Database.Entities.Navigator
{
    [Table("navigator_categories"), Index(nameof(Name), IsUnique = true)]
    public class NavigatorCategoryEntity : Entity
    {
        [Column("name"), Required]
        public string Name { get; set; }

        [Column("is_public"), Required, DefaultValueSql("0")]
        public bool IsPublic { get; set; }
    }
}
