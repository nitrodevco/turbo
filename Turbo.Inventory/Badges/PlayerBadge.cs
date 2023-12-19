using Turbo.Database.Entities.Players;
using Turbo.Core.Game.Inventory;

namespace Turbo.Inventory.Badges
{
    public class PlayerBadge(
        PlayerBadgeEntity _badgeEntity) : IPlayerBadge
    {

        public void SetSlotId(int? slotId)
        {
            if (_badgeEntity == null) return;

            if (_badgeEntity.SlotId == slotId) return;

            _badgeEntity.SlotId = slotId;
        }

        public int Id => _badgeEntity.Id;

        public string BadgeCode => _badgeEntity.BadgeCode;

        public int? SlotId => _badgeEntity.SlotId;
    }
}