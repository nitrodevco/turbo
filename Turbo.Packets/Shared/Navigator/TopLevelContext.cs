using System.Collections.Generic;
using Turbo.Core.Game.Navigator;

namespace Turbo.Packets.Shared.Navigator;

public record TopLevelContext : ITopLevelContext
{
    public string SearchCode { get; init; }
    public IList<INavigatorSavedSearch> SavedSearches { get; init; }
}