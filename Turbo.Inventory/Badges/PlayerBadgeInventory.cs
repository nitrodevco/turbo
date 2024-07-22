using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Game;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Storage;
using Turbo.Core.Utilities;
using Turbo.Database.Repositories.Player;
using Turbo.Packets.Outgoing.Inventory.Badges;

namespace Turbo.Inventory.Badges;

public class PlayerBadgeInventory(
    IPlayer _player,
    IStorageQueue _storageQueue,
    IServiceScopeFactory _serviceScopeFactory) : Component, IPlayerBadgeInventory
{
    private readonly object _activeBadgeLock = new();

    private bool _requested;
    public ConcurrentDictionary<string, IPlayerBadge> Badges { get; } = new();
    public IList<IPlayerBadge> ActiveBadges { get; private set; } = [];

    public void ResetActiveBadges()
    {
        foreach (var playerBadge in ActiveBadges)
            if (playerBadge is PlayerBadge badge)
                badge.SetSlotId(null);

        lock (_activeBadgeLock)
        {
            ActiveBadges.Clear();
        }
    }

    public void SetActivedBadges(IDictionary<int, string> badges)
    {
        ResetActiveBadges();

        lock (_activeBadgeLock)
        {
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
    }

    public void SendBadgesToSession(ISession session)
    {
        List<IPlayerBadge> playerBadges = [];

        var count = 0;

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

    protected override async Task OnInit()
    {
        await LoadBadges();
    }

    protected override async Task OnDispose()
    {
        Badges.Clear();

        lock (_activeBadgeLock)
        {
            ActiveBadges.Clear();
        }
    }

    private async Task LoadBadges()
    {
        Badges.Clear();

        lock (_activeBadgeLock)
        {
            ActiveBadges.Clear();
        }

        using var scope = _serviceScopeFactory.CreateScope();

        var playerBadgeRepository = scope.ServiceProvider.GetService<IPlayerBadgeRepository>();

        var entities = await playerBadgeRepository.FindAllByPlayerIdAsync(_player.Id);

        if (entities != null)
        {
            var activeBadges = new List<IPlayerBadge>();

            foreach (var playerBadgeEntity in entities)
            {
                IPlayerBadge playerBadge = new PlayerBadge(_storageQueue, playerBadgeEntity);

                Badges.TryAdd(playerBadge.BadgeCode, playerBadge);

                if (playerBadge.SlotId != null && playerBadge.SlotId > 0) activeBadges.Add(playerBadge);
            }

            if (activeBadges.Count > 0)
                lock (_activeBadgeLock)
                {
                    ActiveBadges = activeBadges;

                    ((List<IPlayerBadge>)ActiveBadges).Sort((a, b) =>
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