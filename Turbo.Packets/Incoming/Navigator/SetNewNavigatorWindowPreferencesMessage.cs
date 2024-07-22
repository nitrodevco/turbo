using Turbo.Core.Game.Navigator.Constants;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public record SetNewNavigatorWindowPreferencesMessage : IMessageEvent
{
    public int X { get; init; }
    public int Y { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public bool OpenSavedSearches { get; init; }
    public NavigatorResultsMode ResultsMode { get; init; }
}