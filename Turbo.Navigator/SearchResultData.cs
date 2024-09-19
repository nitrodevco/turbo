using System.Collections.Generic;
using Turbo.Core.Game.Navigator;
using Turbo.Core.Game.Rooms;

namespace Turbo.Navigator;

public class SearchResultData : ISearchResultData
{
    public string SearchCode { get; set; }
    public string Text { get; set; }
    public int ActionAllowed { get; set;  }
    public bool ForceClosed { get; set; }
    public int ViewMode { get; set; }
    public ICollection<IRoomDetails> Rooms { get; set; }
}