using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Navigator;

namespace Turbo.Packets.Shared.Navigator
{
    public record TopLevelContext : ITopLevelContext
    {
        public string SearchCode { get; init; }
        public IList<INavigatorSavedSearch> SavedSearches { get; init; }
    }
}
