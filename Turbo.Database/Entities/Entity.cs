using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Turbo.Database.Attributes;

namespace Turbo.Database.Entities
{
    public abstract class Entity
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("date_created"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }

        [Column("date_updated"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateUpdated { get; set; }
    }
}
