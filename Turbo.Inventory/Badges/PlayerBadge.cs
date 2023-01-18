using Turbo.Database.Entities.Players;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Storage;

namespace Turbo.Inventory.Badges
{
    public class PlayerBadge : IPlayerBadge
    {
        private readonly PlayerBadgeEntity _entity;
        private readonly IStorageQueue _storageQueue;

        public PlayerBadge(
            PlayerBadgeEntity badgeEntity,
            IStorageQueue storageQueue)
        {
            _entity = badgeEntity;
            _storageQueue = storageQueue;
        }

        public void SetSlotId(int? slotId)
        {
            if (_entity == null) return;

            if (_entity.SlotId == slotId) return;

            _entity.SlotId = slotId;

            _storageQueue.Add(_entity);
        }

        public int Id => _entity.Id;

        public string BadgeCode => _entity.BadgeCode;

        public int? SlotId => _entity.SlotId;
    }
}