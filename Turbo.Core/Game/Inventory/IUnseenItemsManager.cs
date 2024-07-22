using Turbo.Core.Game.Inventory.Constants;

namespace Turbo.Core.Game.Inventory;

public interface IUnseenItemsManager
{
    public void Commit();
    public void Add(UnseenItemCategory category, params int[] itemIds);
}