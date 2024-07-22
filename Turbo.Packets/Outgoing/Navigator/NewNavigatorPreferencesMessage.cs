using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Navigator;

public record NewNavigatorPreferencesMessage : IComposer
{
    public int WindowX { get; init; }
    public int WindowY { get; init; }
    public int WindowWidth { get; init; }
    public int WindowHeight { get; init; }
    public bool LeftPaneHidden { get; init; }
    public int ResultMode { get; init; }
}