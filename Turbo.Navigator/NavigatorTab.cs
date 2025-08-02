using Turbo.Core.Game.Navigator;
using Turbo.Database.Entities.Navigator;

namespace Turbo.Navigator;

public class NavigatorTopLevelContext(NavigatorTopLevelContextEntity entity) : INavigatorTopLevelContext
{
    public int Id => entity.Id;
    public string SearchCode => entity.SearchCode;
    public bool Visible => entity.Visible;
}