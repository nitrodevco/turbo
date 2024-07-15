using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Turbo.Database.Attributes;

namespace Turbo.Database.Entities.ChatStyles;

[Table("chat_styles")]
public class ChatStyleEntity : Entity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [StringLength(255)]
    public string Description { get; set; }
    
    [Required, DefaultValueSql("0")]
    public int RankRequirement { get; set; }
}