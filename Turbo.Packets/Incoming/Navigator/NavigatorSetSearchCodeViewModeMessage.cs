using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator
{
    public record NavigatorSetSearchCodeViewModeMessage : IMessageEvent
    {
        public string CategoryName { get; init; }
        public int ViewMode { get; init; }
    }
}
