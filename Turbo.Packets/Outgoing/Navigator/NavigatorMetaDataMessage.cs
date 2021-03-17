using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator
{
    public record NavigatorMetaDataMessage : IComposer
    {
        public List<TopLevelContext> TopLevelContexts { get; init; }
    }

    public record TopLevelContext
    {
        public string SearchCode { get; init; }
        public List<SavedSearch> SavedSearches { get; init; }
    }

    public record SavedSearch
    {
        public int Id { get; init; }
        public string SearchCode { get; init; }
        public string Filter { get; init; }
        public string Localization { get; init; }
    }
}
