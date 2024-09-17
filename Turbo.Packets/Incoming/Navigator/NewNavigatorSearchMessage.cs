using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public record NewNavigatorSearchMessage : IMessageEvent
{
    public string SearchCodeOriginal { get; init; }
    public string FilteringData { get; init; }
}