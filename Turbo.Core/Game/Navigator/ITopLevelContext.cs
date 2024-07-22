using System.Collections.Generic;

namespace Turbo.Core.Game.Navigator;

public interface ITopLevelContext
{
    public string SearchCode { get; }
    public IList<INavigatorSavedSearch> SavedSearches { get; }
}