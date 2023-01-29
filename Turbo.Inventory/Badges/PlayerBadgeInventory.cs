using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Storage;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Players;
using Turbo.Database.Repositories.Player;
using Turbo.Packets.Outgoing.Inventory.Badges;

namespace Turbo.Inventory.Badges
{
    public class PlayerBadgeInventory : Component, IPlayerBadgeInventory
    {
        private readonly IPlayer _player;
        private readonly IStorageQueue _storageQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IDictionary<string, IPlayerBadge> Badges { get; private set; }
        public IList<IPlayerBadge> ActiveBadges { get; private set; }

        private bool _requested;

        public PlayerBadgeInventory(
            IPlayer player,
            IStorageQueue storageQueue,
            IServiceScopeFactory serviceScopeFactory)
        {
            _player = player;
            _storageQueue = storageQueue;
            _serviceScopeFactory = serviceScopeFactory;

            Badges = new Dictionary<string, IPlayerBadge>();
            ActiveBadges = new List<IPlayerBadge>();
        }

        protected override async Task OnInit()
        {
            await LoadBadges();
        }

        protected override async Task OnDispose()
        {
            Badges.Clear();
            ActiveBadges.Clear();
        }

        public void ResetActiveBadges()
        {
            foreach (var playerBadge in ActiveBadges)
            {
                if (playerBadge is PlayerBadge badge) badge.SetSlotId(null);
            }

            ActiveBadges.Clear();
        }

        public void SetActivedBadges(IDictionary<int, string> badges)
        {
            ResetActiveBadges();

            foreach (var slotId in badges.Keys)
            {
                var badgeCode = badges[slotId];

                if (badgeCode == null || badgeCode.Length == 0) continue;

                var playerBadge = Badges[badgeCode];

                if (playerBadge == null) continue;

                if (playerBadge is PlayerBadge badge) badge.SetSlotId(slotId);

                ActiveBadges.Add(playerBadge);

                if (ActiveBadges.Count == DefaultSettings.MaxActiveBadges) return;
            }
        }

        public void SendBadgesToSession(ISession session)
        {
            List<IPlayerBadge> playerBadges = new();

            int count = 0;

            foreach (var playerBadge in Badges.Values)
            {
                playerBadges.Add(playerBadge);

                count++;

                if (count == DefaultSettings.BadgesPerFragment)
                {
                    session.Send(new BadgeMessage
                    {
                        Badges = playerBadges,
                        ActiveBadges = ActiveBadges
                    });

                    playerBadges.Clear();
                    count = 0;
                }
            }

            if (count <= 0) return;

            session.Send(new BadgeMessage
            {
                Badges = playerBadges,
                ActiveBadges = ActiveBadges
            });

            _requested = true;
        }

        private async Task LoadBadges()
        {
            Badges.Clear();
            ActiveBadges.Clear();

            List<PlayerBadgeEntity> entities = new();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var playerBadgeRepository = scope.ServiceProvider.GetService<IPlayerBadgeRepository>();

                if (playerBadgeRepository != null)
                {
                    entities = await playerBadgeRepository.FindAllByPlayerIdAsync(_player.Id);
                }
            }

            if (entities != null)
            {
                foreach (var playerBadgeEntity in entities)
                {
                    IPlayerBadge playerBadge = new PlayerBadge(playerBadgeEntity, _storageQueue);

                    Badges.Add(playerBadge.BadgeCode, playerBadge);

                    if (playerBadge.SlotId != null && playerBadge.SlotId > 0) ActiveBadges.Add(playerBadge);

                    if (ActiveBadges is List<IPlayerBadge> activeBadgeList)
                    {
                        activeBadgeList.Sort((a, b) =>
                        {
                            var aSlotId = a.SlotId ?? 0;
                            var bSlotId = b.SlotId ?? 0;

                            if (bSlotId > aSlotId) return 1;

                            if (bSlotId < aSlotId) return -1;

                            return 0;
                        });
                    }
                }
            }
        }
    }
}

