using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public record NewNavigatorSearchMessage : IMessageEvent
{
    public string View { get; init; }
    public string Query { get; init; }
}