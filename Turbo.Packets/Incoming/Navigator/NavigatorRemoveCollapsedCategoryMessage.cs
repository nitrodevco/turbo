using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public record NavigatorRemoveCollapsedCategoryMessage : IMessageEvent
{
    public string CategoryName { get; init; }
}