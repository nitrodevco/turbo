using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class EditEventMessage : IMessageEvent
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
}