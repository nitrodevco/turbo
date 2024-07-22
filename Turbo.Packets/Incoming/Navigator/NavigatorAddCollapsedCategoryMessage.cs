using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public record NavigatorAddCollapsedCategoryMessage : IMessageEvent
{
    public string CategoryName { get; init; }
}