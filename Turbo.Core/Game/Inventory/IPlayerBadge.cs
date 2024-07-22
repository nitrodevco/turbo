namespace Turbo.Core.Game.Inventory;

public interface IPlayerBadge
{
    public int Id { get; }
    public string BadgeCode { get; }
    public int? SlotId { get; }
}