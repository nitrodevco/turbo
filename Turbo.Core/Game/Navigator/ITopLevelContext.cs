using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Navigator
{
    public interface ITopLevelContext
    {
        public string SearchCode { get; }
        public IList<INavigatorSavedSearch> SavedSearches { get; }
    }
}
