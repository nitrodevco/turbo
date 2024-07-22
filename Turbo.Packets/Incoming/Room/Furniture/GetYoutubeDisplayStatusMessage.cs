using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record GetYoutubeDisplayStatusMessage : IMessageEvent
{
    public int ItemId { get; init; }
}