using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Navigator;

public class CancelEventMessage : IMessageEvent
{
    public int AdvertisementId { get; init; }
}