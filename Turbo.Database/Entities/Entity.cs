using System;
using System.ComponentModel.DataAnnotations.Schema;

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
