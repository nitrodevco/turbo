using Turbo.Core.Game.Navigator;
using Turbo.Core.Database.Entities.Navigator;

namespace Turbo.Navigator;

public class NavigatorEventCategory(NavigatorEventCategoryEntity entity) : INavigatorEventCategory
{
    public int Id => entity.Id;
    public string Name => entity.Name;
    public bool Visible => entity.Visible;
}