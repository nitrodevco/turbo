using System.Collections.Generic;
using Turbo.Core.Game.Rooms;

namespace Turbo.Core.Game.Navigator;

public interface ISearchResultData
{
    string SearchCode { get; }
    string Text { get; }
    int ActionAllowed { get; }
    bool ForceClosed { get; }
    int ViewMode { get; }
    ICollection<IRoomDetails> Rooms { get; }
}