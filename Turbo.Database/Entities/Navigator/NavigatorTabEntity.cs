using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Attributes;
using System.Xml.Linq;

namespace Turbo.Database.Entities.Navigator
{
    [Table("navigator_tabs"), Index(nameof(SearchCode), IsUnique = true)]
    public class NavigatorTabEntity : Entity
    {
        [Column("search_code"), Required]
        public string SearchCode { get; set; }
    }
}
