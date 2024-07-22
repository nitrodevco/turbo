using Turbo.Core.Game.Inventory;

namespace Turbo.Core.Database.Dtos;

public class PlayerBadgeDto : IPlayerBadge
{
    public int Id { get; set; }
    public string BadgeCode { get; set; }
    public int? SlotId { get; set; }
}