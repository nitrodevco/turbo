using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public record NavigatorAddSavedSearchMessage : IMessageEvent
{
    public string SearchCode { get; init; }
    public string Filter { get; init; }
}