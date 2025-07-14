using Turbo.Core.Database.Entities.Navigator;
using Turbo.Core.Game.Navigator;

namespace Turbo.Navigator;

public class NavigatorTopLevelContext(NavigatorTopLevelContextEntity entity) : INavigatorTopLevelContext
{
    public int Id => entity.Id;
    public string SearchCode => entity.SearchCode;
    public bool Visible => entity.Visible;
}