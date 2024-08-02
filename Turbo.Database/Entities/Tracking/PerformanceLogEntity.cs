using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Database.Entities.Tracking;

[Table("performance_logs")]
[Index(nameof(IPAddress))]
[Index(nameof(ElapsedTime))]
public class PerformanceLogEntity
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] [Column("id")] public int Id { get; set; }
    
    [Column("elapsed_time")] [Required] public int ElapsedTime { get; set; }

    [Column("user_agent")] [Required] public string UserAgent { get; set; }

    [Column("flash_version")] [Required] public string FlashVersion { get; set; }

    [Column("os")] [Required] public string OS { get; set; }

    [Column("browser")] [Required] public string Browser { get; set; }

    [Column("is_debugger")] [Required] public bool IsDebugger { get; set; }

    [Column("memory_usage")] [Required] public int MemoryUsage { get; set; }

    [Column("garbage_collections")] [Required] public int GarbageCollections { get; set; }

    [Column("average_frame_rate")] [Required] public int AverageFrameRate { get; set; }

    [Column("ip_address")] [Required] [MaxLength(45)] public string IPAddress { get; set; }
}