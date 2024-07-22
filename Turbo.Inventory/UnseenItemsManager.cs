using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Inventory.Constants;
using Turbo.Core.Game.Players;
using Turbo.Packets.Outgoing.Notifications;

namespace Turbo.Inventory;

public class UnseenItemsManager(IPlayer player) : IUnseenItemsManager
{
    private readonly IPlayer _player = player;

    private readonly IDictionary<UnseenItemCategory, IList<int>> _unseenCategories =
        new Dictionary<UnseenItemCategory, IList<int>>();

    public void Commit()
    {
        _player.Session?.Send(new UnseenItemsMessage
        {
            Categories = _unseenCategories
        });

        _unseenCategories.Clear();
    }

    public void Add(UnseenItemCategory category, params int[] itemIds)
    {
        if (itemIds.Length == 0) return;

        if (_unseenCategories.TryGetValue(category, out var unseenItems))
        {
            foreach (var itemId in itemIds)
            {
                if (unseenItems.Contains(itemId)) continue;

                unseenItems.Add(itemId);
            }

            return;
        }

        _unseenCategories.Add(category, itemIds.ToList());

        Commit();
    }
}