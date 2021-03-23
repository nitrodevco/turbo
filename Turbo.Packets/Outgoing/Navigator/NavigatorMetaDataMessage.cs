using System.Collections.Generic;
using Turbo.Core.Packets.Messages;
using Turbo.Packets.Shared.Navigator;

namespace Turbo.Packets.Outgoing.Navigator
{
    public record NavigatorMetaDataMessage : IComposer
    {
        public List<TopLevelContext> TopLevelContexts { get; init; }
    }

    public record TopLevelContext
    {
        public string SearchCode { get; init; }
        public List<NavigatorSavedSearch> SavedSearches { get; init; }
    }
}
