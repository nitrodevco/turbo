using Turbo.Core.Game.Navigator;
using Turbo.Core.Database.Entities.Navigator;

namespace Turbo.Navigator;

public class NavigatorEventCategory(NavigatorEventCategoryEntity _entity) : INavigatorEventCategory
{
    public int Id => _entity.Id;
    public string Name => _entity.Name;
    public bool Enabled => _entity.Enabled;
}