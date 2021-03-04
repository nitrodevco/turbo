using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Database.Entities
{
    public abstract class Entity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("date_created")]
        public DateTime DateCreated { get; set; }

        [Column("date_updated")]
        public DateTime DateUpdated { get; set; }
}
}
