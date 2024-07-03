using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Navigator;
using Turbo.Database.Entities.Navigator;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Shared.Navigator;

namespace Turbo.Navigator
{
    public class NavigatorTab(NavigatorTabEntity _entity) : INavigatorTab
    {
        public int Id => _entity.Id;

        public string SearchCode => _entity.SearchCode;

        public ITopLevelContext TopLevelContext => new TopLevelContext { SearchCode = SearchCode };
    }
}
