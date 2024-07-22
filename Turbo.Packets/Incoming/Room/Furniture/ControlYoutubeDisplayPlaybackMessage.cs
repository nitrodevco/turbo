using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Incoming.Room.Furniture;

public record ControlYoutubeDisplayPlaybackMessage : IMessageEvent
{
    public int ItemId { get; init; }
    public int State { get; init; }
}