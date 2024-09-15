using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turbo.Core.Database.Entities;

public abstract class Entity
{
    [Key] [Column("id")] public int Id { get; set; }

    [Column("created_at")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }
    
    [Column("deleted_at")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? DeletedAt { get; set; }
}