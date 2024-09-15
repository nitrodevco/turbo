using Turbo.Core.Game.Navigator;
using Turbo.Core.Database.Entities.Navigator;
using Turbo.Packets.Shared.Navigator;

namespace Turbo.Navigator;

public class NavigatorTab(NavigatorTabEntity _entity) : INavigatorTab
{
    public int Id => _entity.Id;

    public string SearchCode => _entity.SearchCode;

    public ITopLevelContext TopLevelContext => new TopLevelContext { SearchCode = SearchCode };
}