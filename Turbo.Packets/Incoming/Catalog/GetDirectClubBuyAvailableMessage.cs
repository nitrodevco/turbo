using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Catalog;

public record GetDirectClubBuyAvailableMessage : IMessageEvent
{
    public int Days { get; init; }
}