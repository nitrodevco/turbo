using System.Collections.Generic;
using Turbo.Core.Packets.Messages;
using Turbo.Packets.Shared.Navigator;

namespace Turbo.Packets.Outgoing.Navigator
{
    public record NavigatorSavedSearchesMessage : IComposer
    {
        public List<NavigatorSavedSearch> SavedSearches {get; init;}
    }
}
