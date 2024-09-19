using System.Collections.Generic;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public class NavigatorSearchResultBlocksMessage : IComposer
{
    public string SearchCode { get; init; }
    public string Filtering { get; init; }

    public ICollection<ISearchResultData> Results { get; init; } = new List<ISearchResultData>();
}